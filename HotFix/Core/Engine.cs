using System;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Core
{
    public class Engine
    {
        public Func<IConfiguration, IClock> Clocks { get; set; }
        public Func<IConfiguration, ITransport> Transports { get; set; }

        public Engine()
        {
            Clocks = c => new RealTimeClock();
            Transports = c => new TcpTransport(c.Host, c.Port);
        }

        public Session Initiate(IConfiguration configuration)
        {
            var clock = Clocks(configuration);
            var transport = Transports(configuration);

            return new Session(configuration, clock, transport);
        }
    }
}