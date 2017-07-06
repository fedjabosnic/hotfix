using System;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Core
{
    public class Engine
    {
        /// <summary> A factory method for clocks </summary>
        public Func<IConfiguration, IClock> Clocks { get; set; }
        /// <summary> A factory method for loggers </summary>
        public Func<IConfiguration, ILogger> Loggers { get; set; }
        /// <summary> A factory method for transports </summary>
        public Func<IConfiguration, ITransport> Transports { get; set; }

        /// <summary> Gets or sets the buffer size used for transport and session buffering </summary>
        public int BufferSize { get; set; }
        /// <summary> Gets or sets the maximum length of fix messages </summary>
        public int MaxMessageLength { get; set; }
        /// <summary> Gets or sets the maximum fields in fix messages </summary>
        public int MaxMessageFields { get; set; }

        public Engine()
        {
            var clock = new RealTimeClock();

            Clocks = c => clock;
            Loggers = c => c.LogFile != null ? new FileLogger(clock, c.LogFile) : null;
            Transports = c => TcpTransport.Create(c.Role == Role.Acceptor, c.Host, c.Port);

            BufferSize = 65536;
            MaxMessageLength = 4096;
            MaxMessageFields = 1024;
        }

        /// <summary>
        /// Opens a session along with the relevant transport.
        /// <remarks>
        /// This is a blocking operation which directly connects the underlying transport.
        /// </remarks>
        /// </summary>
        /// <param name="configuration">The session configuration.</param>
        /// <returns>The connected session.</returns>
        public Session Open(IConfiguration configuration)
        {
            var clock = Clocks(configuration);
            var logger = Loggers(configuration);
            var transport = Transports(configuration);

            var session = new Session(configuration, clock, transport, BufferSize, MaxMessageLength, MaxMessageFields);

            if (logger != null)
            {
                session.Channel.Inbound = logger.Inbound;
                session.Channel.Outbound = logger.Outbound;
            }

            return session;
        }
    }
}