using System;
using System.Diagnostics;
using System.Threading;
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
            Transports = c => TcpTransport.Create(clock, c.Role == Role.Acceptor, c.Host, c.Port);

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

            var session = new Session(configuration, clock, transport, logger, BufferSize, MaxMessageLength, MaxMessageFields);

            return session;
        }

        /// <summary> 
        /// Runs a session for the provided configuration, allowing logon, logout and message handler to be specified. 
        /// <remarks> 
        /// The logon and logout callbacks are invoked after the session has successfully logged on or out. 
        /// The message handler should return false for every message it does not handle so that the session can deal with it. 
        /// </remarks> 
        /// </summary> 
        /// <param name="configuration">The session configuration.</param> 
        /// <param name="logon">The logon callback.</param> 
        /// <param name="logout">The logout callback.</param> 
        /// <param name="inbound">The inbound message callback.</param>
        /// <param name="outbound">The outbound message callback.</param> 
        public void Run(IConfiguration configuration, Action<Session> logon = null, Action<Session> logout = null, Action<Session, FIXMessage> inbound = null, Action<Session, FIXMessageWriter> outbound = null)
        {
            while (true)
            {
                try
                {
                    var clock = Clocks(configuration);
                    var schedule = configuration.Schedules.GetActive(clock.Time);

                    if (schedule == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    using (var session = this.Open(configuration))
                    {
                        session.LoggedOn += logon;
                        session.LoggedOut += logout;
                        session.Received += inbound;
                        session.Sent += outbound;

                        session.Logon();

                        while (session.Active && clock.Time < schedule.Close)
                        {
                            session.Receive();
                        }

                        session.Logout();

                        session.Sent -= outbound;
                        session.Received -= inbound;
                        session.LoggedOut -= logout;
                        session.LoggedOn -= logon;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    
                    Thread.Sleep(10000);
                }
            }
        }
    }
}