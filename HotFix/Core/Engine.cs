using System;
using System.Runtime.CompilerServices;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Core
{
    public class Engine
    {
        public static IClock Clock { get; set; }
        public Func<IConfiguration, ITransport> Transports { get; set; }

        public Engine()
        {
            Clock = new RealTimeClock();
            Transports = c => new TcpTransport(c.Host, c.Port);
        }

        public void Run(IConfiguration configuration)
        {
            var transport = Transports(configuration);
            var channel = new Channel(transport);

            var inbound = new FIXMessage();
            var outbound = new FIXMessageWriter(1024, configuration.Version);

            HandleLogon(configuration, channel, inbound, outbound);

            while (true)
            {
                if (inbound.Valid)
                {
                    if (!inbound[8].Is(configuration.Version)) throw new EngineException("Unexpected begin string received");
                    if (!inbound[49].Is(configuration.Target)) throw new EngineException("Unexpected comp id received");
                    if (!inbound[56].Is(configuration.Sender)) throw new EngineException("Unexpected comp id received");

                    if (inbound[34].AsLong < configuration.InboundSeqNum) throw new EngineException("Sequence number too low");
                    if (inbound[34].AsLong > configuration.InboundSeqNum) throw new EngineException("Sequence number too high");

                    // Process message
                    Console.WriteLine("Processing: " + inbound[35].AsString);

                    configuration.InboundSeqNum++;
                    configuration.InboundTimestamp = Clock.Time;
                }

                if (Clock.Time - configuration.OutboundTimestamp > TimeSpan.FromSeconds(5))
                {
                    outbound.Prepare("0");
                    outbound.Set(34, configuration.OutboundSeqNum);
                    outbound.Set(52, Clock.Time);
                    outbound.Set(49, configuration.Sender);
                    outbound.Set(56, configuration.Target);
                    outbound.Build();

                    Send(configuration, channel, outbound);
                }

                inbound.Clear();

                channel.Read(inbound);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(IConfiguration configuration, Channel channel, FIXMessageWriter message)
        {
            channel.Write(message);

            configuration.OutboundSeqNum++;
            configuration.OutboundTimestamp = Clock.Time;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HandleLogon(IConfiguration configuration, Channel channel, FIXMessage inbound, FIXMessageWriter outbound)
        {
            outbound.Prepare("A");
            outbound.Set(34, 1);
            outbound.Set(52, Clock.Time);
            outbound.Set(49, configuration.Sender);
            outbound.Set(56, configuration.Target);
            outbound.Set(108, configuration.HeartbeatInterval);
            outbound.Set(98, 0);
            outbound.Set(141, "Y");
            outbound.Build();

            Send(configuration, channel, outbound);

            while (Clock.Time - configuration.OutboundTimestamp < TimeSpan.FromSeconds(10))
            {
                channel.Read(inbound);

                if (inbound.Valid)
                {
                    if (!inbound[35].Is("A")) throw new EngineException("Unexpected first message received (expected a logon)");
                    if (!inbound[108].Is(configuration.HeartbeatInterval)) throw new EngineException("Unexpected heartbeat interval received");
                    if (!inbound[ 98].Is(0)) throw new EngineException("Unexpected encryption method received");
                    if (!inbound[141].Is("Y")) throw new EngineException("Unexpected reset on logon received");

                    return;
                }
            }

            throw new EngineException("Logon response not received on time");
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