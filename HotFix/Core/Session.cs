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

        /// <summary> The session configuration </summary>
        public IConfiguration Configuration { get; }
        /// <summary> The session configuration </summary>
        public Channel Channel { get; }
        /// <summary> The session configuration </summary>
        public State State { get; }
        public bool Active { get; private set; }

        /// <summary> The inbound fix message </summary>
        public FIXMessage Inbound;
        /// <summary> The outbound fix message </summary>
        public FIXMessageWriter Outbound;

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="configuration">The session configuration</param>
        /// <param name="clock">The session clock</param>
        /// <param name="transport">The session transport</param>
        /// <param name="bufferSize">The buffersize to use for buffering</param>
        /// <param name="maxMessageLength">The maximum supported length of a fix message</param>
        /// <param name="maxMessageFields">The maximum supported number of fields in a fix message</param>
        public Session(IConfiguration configuration, IClock clock, ITransport transport, int bufferSize, int maxMessageLength, int maxMessageFields)
        {
            Clock = clock;
            Configuration = configuration;

            Channel = new Channel(transport, bufferSize);
            State = new State
            {
                InboundSeqNum = configuration.InboundSeqNum,
                OutboundSeqNum = configuration.OutboundSeqNum,
                InboundTimestamp = clock.Time,
                OutboundTimestamp = clock.Time
            };

            Inbound = new FIXMessage(maxMessageLength, maxMessageFields);
            Outbound = new FIXMessageWriter(maxMessageLength);
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
            Outbound.Clear();
            Send("5", Outbound);

            Active = false;
        }

        /// <summary>
        /// Attempts to receive one message over the session.
        /// <remarks>
        /// You should call this continuously as often as possible to process incoming messages and keep the session alive.
        /// </remarks>
        /// </summary>
        public bool Receive()
        {
            var clock = Clock;
            var state = State;
            var channel = Channel;
            var configuration = Configuration;

            var inbound = Inbound;
            var outbound = Outbound;

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

                    if (inbound[35].Is("1")) HandleTestRequest();
                    if (inbound[35].Is("2")) HandleResendRequest();
                    if (inbound[35].Is("4")) HandleSequenceReset();
                    if (inbound[35].Is("5")) HandleLogout();
                    if (inbound[35].Is("A")) HandleLogon();

                    state.InboundSeqNum++;
                    state.InboundTimestamp = clock.Time;
                }
                else
                {
                    if (inbound[34].AsLong < state.InboundSeqNum) throw new EngineException("Sequence number too low");
                    if (inbound[34].AsLong > state.InboundSeqNum) SendResendRequest();
                }
            }

            if (clock.Time - state.OutboundTimestamp > TimeSpan.FromSeconds(configuration.HeartbeatInterval))
            {
                SendHeartbeat();
            }

            if (clock.Time - state.InboundTimestamp > TimeSpan.FromSeconds(configuration.HeartbeatInterval * 1.2))
            {
                if (clock.Time - state.InboundTimestamp > TimeSpan.FromSeconds(configuration.HeartbeatInterval * 2))
                {
                    throw new EngineException("Did not receive any messages for too long");
                }

                if (!state.TestRequestPending)
                {
                    SendTestRequest();
                    state.TestRequestPending = true;
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
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Send(string messageType, FIXMessageWriter message, long seqNum)
        {
            message.Prepare(Configuration.Version, messageType, seqNum, Clock.Time, Configuration.Sender, Configuration.Target);

            Channel.Write(message);

            State.OutboundTimestamp = Clock.Time;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendHeartbeat()
        {
            Outbound.Clear();
            Send("0", Outbound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendTestRequest()
        {
            Outbound.Clear().Set(112, Clock.Time.Ticks);

            Send("1", Outbound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendResendRequest()
        {
            var state = State;

            if (state.Synchronizing) return;

            Outbound.Clear().Set(7, state.InboundSeqNum).Set(16, 0);

            Send("2", Outbound);

            state.Synchronizing = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HandleTestRequest()
        {
            // Prepare and send a heartbeat (with the test request id)
            Outbound.Clear().Set(112, Inbound[112].AsString);

            Send("0", Outbound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HandleResendRequest()
        {
            // Validate request
            if (!Inbound[16].Is(0L)) throw new EngineException("Unsupported resend request received (partial gap fills are not supported)");

            // Prepare and send a gap fill message
            Outbound.Clear().Set(123, "Y").Set(36, State.OutboundSeqNum);

            Send("4", Outbound, Inbound[7].AsLong);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HandleSequenceReset()
        {
            var inbound = Inbound;

            // Validate request
            if (!inbound.Contains(123) || !inbound[123].Is("Y")) throw new Exception("Unsupported sequence reset received (hard reset)");
            if (inbound[36].AsLong <= State.InboundSeqNum) throw new Exception("Invalid sequence reset received (bad new seq num)");

            // Accept the new sequence number
            State.InboundSeqNum = inbound[36].AsLong;
        }

        public void HandleLogout()
        {
            Logout();
        }

        public void HandleLogon()
        {
            throw new EngineException("Logon message received while already logged on");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AcceptLogon()
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
                    if (!Inbound[8].Is(configuration.Version)) throw new EngineException("Unexpected begin string received");
                    if (!Inbound[49].Is(configuration.Target)) throw new EngineException("Unexpected comp id received");
                    if (!Inbound[56].Is(configuration.Sender)) throw new EngineException("Unexpected comp id received");

                    if (!Inbound[35].Is("A")) throw new EngineException("Unexpected first message received (expected a logon)");
                    if (!Inbound[108].Is(Configuration.HeartbeatInterval)) throw new EngineException("Unexpected heartbeat interval received");
                    if (!Inbound[98].Is(0)) throw new EngineException("Unexpected encryption method received");
                    if (!Inbound[141].Is("Y")) throw new EngineException("Unexpected reset on logon received");

                    if (Inbound[34].AsLong < state.InboundSeqNum) throw new EngineException("Sequence number too low");

                    outbound
                        .Clear()
                        .Set(108, Configuration.HeartbeatInterval)
                        .Set(98, 0)
                        .Set(141, "Y");

                    Send("A", outbound);

                    state.InboundSeqNum++;
                    state.InboundTimestamp = Clock.Time;

                    if (Inbound[34].AsLong > state.InboundSeqNum)
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
        public void InitiateLogon()
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
                    if (!Inbound[8].Is(configuration.Version)) throw new EngineException("Unexpected begin string received");
                    if (!Inbound[49].Is(configuration.Target)) throw new EngineException("Unexpected comp id received");
                    if (!Inbound[56].Is(configuration.Sender)) throw new EngineException("Unexpected comp id received");

                    if (!inbound[35].Is("A")) throw new EngineException("Unexpected first message received (expected a logon)");
                    if (!inbound[108].Is(configuration.HeartbeatInterval)) throw new EngineException("Unexpected heartbeat interval received");
                    if (!inbound[98].Is(0)) throw new EngineException("Unexpected encryption method received");
                    if (!inbound[141].Is("Y")) throw new EngineException("Unexpected reset on logon received");

                    if (!Inbound[34].Is(state.InboundSeqNum))
                    {
                        if (Inbound[34].AsLong < state.InboundSeqNum) throw new EngineException("Sequence number too low");
                        if (Inbound[34].AsLong > state.InboundSeqNum)
                        {
                            SendResendRequest();
                            return;
                        }
                    }

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
            Channel.Transport.Dispose();
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