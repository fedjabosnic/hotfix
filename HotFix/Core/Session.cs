using System;
using System.Runtime.CompilerServices;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Core
{
    public class Session : IDisposable
    {
        /// <summary> The session clock </summary>
        public IClock Clock { get; }
        /// <summary> The session logger </summary>
        public ILogger Logger { get; }

        /// <summary> The session configuration </summary>
        public Configuration Configuration { get; }
        /// <summary> The session channel </summary>
        public Channel Channel { get; }
        /// <summary> The session state </summary>
        public State State { get; }

        /// <summary> Whether the session is active </summary>
        public bool Active { get; private set; }

        /// <summary> The inbound fix message </summary>
        public FIXMessage Inbound;
        /// <summary> The outbound fix message </summary>
        public FIXMessageWriter Outbound;

        /// <summary>An event raised when the session has logged on.</summary>
        public event Action<Session> LoggedOn;
        /// <summary>An event raised when the session has logged out.</summary>
        public event Action<Session> LoggedOut;
        /// <summary>An event raised when the session has received a message.</summary>
        public event Action<Session, FIXMessage> Received;
        /// <summary>An event raised when the session has sent a message.</summary>
        public event Action<Session, FIXMessageWriter> Sent;

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="configuration">The session configuration</param>
        /// <param name="clock">The session clock</param>
        /// <param name="transport">The session transport</param>
        /// <param name="logger">The session logger</param>
        /// <param name="bufferSize">The buffersize to use for buffering</param>
        /// <param name="maxMessageLength">The maximum supported length of a fix message</param>
        /// <param name="maxMessageFields">The maximum supported number of fields in a fix message</param>
        public Session(Configuration configuration, IClock clock, ITransport transport, ILogger logger, int bufferSize, int maxMessageLength, int maxMessageFields)
        {
            Clock = clock;
            Logger = logger;
            Configuration = configuration;

            Channel = new Channel(transport, bufferSize);

            State = new State
            {
                InboundSeqNum = configuration.InboundSeqNum,
                OutboundSeqNum = configuration.OutboundSeqNum,
                InboundTimestamp = clock.Time,
                OutboundTimestamp = clock.Time,
                HeartbeatInterval = TimeSpan.FromSeconds(configuration.HeartbeatInterval * 1.0),
                HeartbeatTimeoutMin = TimeSpan.FromSeconds(configuration.HeartbeatInterval * 1.2),
                HeartbeatTimeoutMax = TimeSpan.FromSeconds(configuration.HeartbeatInterval * 2.0)
            };

            if (Logger != null)
            {
                Channel.Inbound = Logger.Inbound;
                Channel.Outbound = Logger.Outbound;
            }

            Inbound = new FIXMessage(maxMessageLength, maxMessageFields) { Clock = Clock };
            Outbound = new FIXMessageWriter(maxMessageLength) { Clock = Clock };
        }

        /// <summary>
        /// Executes the session's logon process.
        /// <remarks>
        /// This action is idempotent, calling this multiple times has no ill effect.
        /// </remarks>
        /// </summary>
        public void Logon()
        {
            if (Active) return;

            switch (Configuration.Role)
            {
                case Role.Acceptor:
                    AcceptLogon();
                    break;
                case Role.Initiator:
                    InitiateLogon();
                    break;
                default:
                    throw new Exception("Unrecognised session role");
            }

            Active = true;

            LoggedOn?.Invoke(this);
        }

        /// <summary>
        /// Executes the session's logout process.
        /// <remarks>
        /// This action is idempotent, calling this multiple times has no ill effect.
        /// </remarks>
        /// </summary>
        public void Logout()
        {
            if (!Active) return;

            // Send a logout message
            Send("5", Outbound.Clear());

            Active = false;

            LoggedOut?.Invoke(this);
        }

        /// <summary>
        /// Attempts to receive one message over the session.
        /// <remarks>
        /// You should call this continuously as often as possible to process incoming messages and keep the session alive.
        /// </remarks>
        /// </summary>
        /// <returns>A boolean indicating whether a valid message was successfully received.</returns>
        public bool Receive()
        {
            var clock = Clock;
            var state = State;
            var channel = Channel;
            var configuration = Configuration;

            var inbound = Inbound;

            inbound.Clear();

            channel.Read(inbound);

            if (inbound.Valid)
            {
                if (!inbound[8].Is(configuration.Version)) throw new EngineException("Unexpected begin string received");
                if (!inbound[49].Is(configuration.Target)) throw new EngineException("Unexpected comp id received");
                if (!inbound[56].Is(configuration.Sender)) throw new EngineException("Unexpected comp id received");

                if (inbound[34].Is(state.InboundSeqNum))
                {
                    state.Synchronizing = false;
                    state.TestRequestPending = false;

                    Received?.Invoke(this, inbound);

                    var type = inbound[35];

                    if (type.Is("0")) HandleHeartbeat();
                    else if (type.Is("1")) HandleTestRequest();
                    else if (type.Is("2")) HandleResendRequest();
                    else if (type.Is("4")) HandleSequenceReset();
                    else if (type.Is("5")) HandleLogout();
                    else if (type.Is("A")) HandleLogon();

                    state.InboundSeqNum++;
                    state.InboundTimestamp = clock.Time;
                }
                else
                {
                    if (inbound[34].AsLong < state.InboundSeqNum) throw new EngineException("Sequence number too low");
                    if (inbound[34].AsLong > state.InboundSeqNum) SendResendRequest();
                }
            }

            if (state.HeartbeatInterval != TimeSpan.Zero)
            {
                if (clock.Time - state.OutboundTimestamp > state.HeartbeatInterval)
                {
                    SendHeartbeat();
                }

                if (clock.Time - state.InboundTimestamp > state.HeartbeatTimeoutMin)
                {
                    if (clock.Time - state.InboundTimestamp > state.HeartbeatTimeoutMax)
                    {
                        throw new EngineException("Did not receive any messages for too long");
                    }

                    if (!state.TestRequestPending) SendTestRequest();
                }
            }

            return inbound.Valid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(string messageType, FIXMessageWriter message)
        {
            var state = State;

            message.Prepare(Configuration.Version, messageType, state.OutboundSeqNum, Clock.Time, Configuration.Sender, Configuration.Target);

            Channel.Write(message);

            state.OutboundSeqNum++;
            state.OutboundTimestamp = Clock.Time;

            Sent?.Invoke(this, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Send(string messageType, FIXMessageWriter message, long seqNum)
        {
            message.Prepare(Configuration.Version, messageType, seqNum, Clock.Time, Configuration.Sender, Configuration.Target);

            Channel.Write(message);

            State.OutboundTimestamp = Clock.Time;

            Sent?.Invoke(this, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SendHeartbeat()
        {
            Outbound.Clear();
            Send("0", Outbound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SendTestRequest()
        {
            Outbound.Clear().Set(112, Clock.Time.Ticks);

            Send("1", Outbound);

            State.TestRequestPending = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SendResendRequest()
        {
            var state = State;

            if (state.Synchronizing) return;

            Outbound.Clear().Set(7, state.InboundSeqNum).Set(16, 0);

            Send("2", Outbound);

            state.Synchronizing = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleHeartbeat()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleTestRequest()
        {
            // Prepare and send a heartbeat (with the test request id)
            Outbound.Clear().Set(112, Inbound[112].AsString);

            Send("0", Outbound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleResendRequest()
        {
            // Validate request
            if (!Inbound[16].Is(0L)) throw new EngineException("Unsupported resend request received (partial gap fills are not supported)");

            // Prepare and send a gap fill message
            Outbound.Clear().Set(123, "Y").Set(36, State.OutboundSeqNum);

            Send("4", Outbound, Inbound[7].AsLong);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleSequenceReset()
        {
            var inbound = Inbound;

            // Validate request
            if (!inbound.Contains(123) || !inbound[123].Is("Y")) throw new Exception("Unsupported sequence reset received (hard reset)");
            if (inbound[36].AsLong <= State.InboundSeqNum) throw new Exception("Invalid sequence reset received (bad new seq num)");

            // Accept the new sequence number
            State.InboundSeqNum = inbound[36].AsLong;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleLogout()
        {
            Logout();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HandleLogon()
        {
            throw new EngineException("Logon message received while already logged on");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AcceptLogon()
        {
            var state = State;
            var channel = Channel;
            var configuration = Configuration;

            var inbound = Inbound;
            var outbound = Outbound;

            while (Clock.Time - state.OutboundTimestamp < TimeSpan.FromSeconds(10))
            {
                channel.Read(inbound);

                if (inbound.Valid)
                {
                    if (!inbound[8].Is(configuration.Version)) throw new EngineException("Unexpected begin string received");
                    if (!inbound[49].Is(configuration.Target)) throw new EngineException("Unexpected comp id received");
                    if (!inbound[56].Is(configuration.Sender)) throw new EngineException("Unexpected comp id received");

                    if (!inbound[35].Is("A")) throw new EngineException("Unexpected first message received (expected a logon)");
                    if (!inbound[108].Is(Configuration.HeartbeatInterval)) throw new EngineException("Unexpected heartbeat interval received");
                    if (!inbound[98].Is(0)) throw new EngineException("Unexpected encryption method received");
                    if (!inbound[141].Is("Y")) throw new EngineException("Unexpected reset on logon received");

                    if (inbound[34].AsLong < state.InboundSeqNum) throw new EngineException("Sequence number too low");

                    Received?.Invoke(this, inbound);

                    outbound
                        .Clear()
                        .Set(108, Configuration.HeartbeatInterval)
                        .Set(98, 0)
                        .Set(141, "Y");

                    Send("A", outbound);

                    state.InboundSeqNum++;
                    state.InboundTimestamp = Clock.Time;

                    if (inbound[34].AsLong > state.InboundSeqNum)
                    {
                        state.InboundSeqNum--; // HACK
                        SendResendRequest();
                    }

                    return;
                }
            }

            throw new EngineException("Logon request not received on time");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitiateLogon()
        {
            var state = State;
            var channel = Channel;
            var configuration = Configuration;

            var inbound = Inbound;
            var outbound = Outbound;

            outbound
                .Clear()
                .Set(108, Configuration.HeartbeatInterval)
                .Set(98, 0)
                .Set(141, "Y");

            Send("A", outbound);

            while (Clock.Time - state.OutboundTimestamp < TimeSpan.FromSeconds(10))
            {
                channel.Read(inbound);

                if (inbound.Valid)
                {
                    if (!inbound[8].Is(configuration.Version)) throw new EngineException("Unexpected begin string received");
                    if (!inbound[49].Is(configuration.Target)) throw new EngineException("Unexpected comp id received");
                    if (!inbound[56].Is(configuration.Sender)) throw new EngineException("Unexpected comp id received");

                    if (!inbound[35].Is("A")) throw new EngineException("Unexpected first message received (expected a logon)");
                    if (!inbound[108].Is(configuration.HeartbeatInterval)) throw new EngineException("Unexpected heartbeat interval received");
                    if (!inbound[98].Is(0)) throw new EngineException("Unexpected encryption method received");
                    if (!inbound[141].Is("Y")) throw new EngineException("Unexpected reset on logon received");

                    if (!inbound[34].Is(state.InboundSeqNum))
                    {
                        if (inbound[34].AsLong < state.InboundSeqNum) throw new EngineException("Sequence number too low");
                        if (inbound[34].AsLong > state.InboundSeqNum)
                        {
                            Received?.Invoke(this, inbound);

                            SendResendRequest();
                            return;
                        }
                    }

                    Received?.Invoke(this, inbound);

                    state.InboundSeqNum++;
                    state.InboundTimestamp = Clock.Time;

                    return;
                }
            }

            throw new EngineException("Logon response not received on time");
        }

        /// <summary>
        /// Disconnects the underlying transport.
        /// </summary>
        public void Dispose()
        {
            Channel?.Transport?.Dispose();
            Logger?.Dispose();
        }
    }

    public class EngineException : Exception
    {
        public EngineException()
        {

        }

        public EngineException(string message) : base(message)
        {

        }
    }
}