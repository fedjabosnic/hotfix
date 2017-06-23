using System;
using System.Runtime.CompilerServices;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Core
{
    public class Engine
    {
        public static IClock Clock { get; set; }

        public IConfiguration Configuration { get; }
        public Func<IConfiguration, ITransport> Transports { get; set; }

        public Engine(IConfiguration configuration)
        {
            Clock = new RealTimeClock();
            Transports = c => new TcpTransport(Configuration.Host, Configuration.Port);

            Configuration = configuration;
        }

        public void Run()
        {
            var transport = Transports(Configuration);
            var channel = new Channel(transport);

            var inbound = new FIXMessage();
            var outbound = new FIXMessageWriter(1024, Configuration.Version);

            var last = Engine.Clock.Time;


            SendLogonRequest(channel, outbound);

            if (!AwaitLogonResponse(channel, inbound))
            {
                transport.Dispose();

                Console.WriteLine("Logon attempt failed, closing...");
                Console.ReadLine();

                return;
            }

            Console.WriteLine("Logged on...");

            inbound.Clear();

            while (true)
            {
                channel.Read(inbound);

                if (inbound.Valid)
                {
                    Console.WriteLine("Processing: " + inbound[35].AsString);
                }

                if (Engine.Clock.Time - last > TimeSpan.FromSeconds(5))
                {
                    outbound.Prepare("0");
                    outbound.Set(34, Configuration.OutboundSeqNum);
                    outbound.Set(52, Engine.Clock.Time);
                    outbound.Set(49, Configuration.Sender);
                    outbound.Set(56, Configuration.Target);
                    outbound.Build();

                    channel.Write(outbound);
                    Configuration.OutboundSeqNum++;

                    last = Engine.Clock.Time;
                }

                inbound.Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send(Channel channel, FIXMessageWriter message)
        {
            channel.Write(message);
            Configuration.OutboundSeqNum++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SendLogonRequest(Channel channel, FIXMessageWriter message)
        {
            message.Prepare("A");
            message.Set(34, 1);
            message.Set(52, Engine.Clock.Time);
            message.Set(49, Configuration.Sender);
            message.Set(56, Configuration.Target);
            message.Set(98, 0);
            message.Set(108, Configuration.HeartbeatInterval);
            message.Set(141, "Y");
            message.Build();

            Send(channel, message);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AwaitLogonResponse(Channel channel, FIXMessage message)
        {
            for (var i = 0; i < 10; i++)
            {
                channel.Read(message);

                if (message.Valid)
                {
                    if (message[35].Is("A"))
                    {
                        // TODO: Validate logon

                        // Validate version
                        // Validate sender
                        // Validate target
                        // Validate 98, 108 and 142

                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
    }
}