using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Core
{
    public class Engine
    {
        /// <summary> A factory method for clocks </summary>
        public Func<Configuration, IClock> Clocks { get; set; }
        /// <summary> A factory method for loggers </summary>
        public Func<Configuration, ILogger> Loggers { get; set; }
        /// <summary> A factory method for transports </summary>
        public Func<Configuration, ITransport> Transports { get; set; }

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
        public Session Open(Configuration configuration)
        {
            var clock = Clocks(configuration);
            var logger = Loggers(configuration);
            var transport = Transports(configuration);

            var session = new Session(configuration, clock, transport, logger, BufferSize, MaxMessageLength, MaxMessageFields);

            return session;
        }

        /// <summary> 
        /// Runs a session for the provided configuration synchronously, allowing callbacks to be specified for different events.
        /// </summary> 
        /// <param name="configuration">The session configuration.</param> 
        /// <param name="logon">Invoked after a session has successfully logged on.</param> 
        /// <param name="logout">Invoked after a session has successfully logged out.</param> 
        /// <param name="inbound">Invoked after a message is received (validated but not consumed by the session).</param>
        /// <param name="outbound">Invoked after a message is sent.</param>
        /// <param name="error">Invoked when the session throws an exception before the session is restarted.</param> 
        public void Run(Configuration configuration, Action<Session> logon = null, Action<Session> logout = null, Action<Session, FIXMessage> inbound = null, Action<Session, FIXMessageWriter> outbound = null, Action<Exception> error = null)
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
                    error?.Invoke(e);
                    
                    Thread.Sleep(10000);
                }
            }
        }

        /// <summary> 
        /// Runs a session for the provided configuration asynchronously, allowing callbacks to be specified for different events.
        /// </summary> 
        /// <param name="configuration">The session configuration.</param>
        /// <param name="token">The cancellation token that stops the session.</param>
        /// <param name="logon">Invoked after a session has successfully logged on.</param> 
        /// <param name="logout">Invoked after a session has successfully logged out.</param> 
        /// <param name="inbound">Invoked after a message is received.</param>
        /// <param name="outbound">Invoked after a message is sent.</param>
        /// <param name="error">Invoked when the session throws an exception - the session is then restarted.</param> 
        public Task RunAsync(Configuration configuration, CancellationToken token, Action<Session> logon = null, Action<Session> logout = null, Action<Session, FIXMessage> inbound = null, Action<Session, FIXMessageWriter> outbound = null, Action<Exception> error = null)
        {
            return Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
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

                            while (session.Active && clock.Time < schedule.Close && !token.IsCancellationRequested)
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

                        error?.Invoke(e);

                        Thread.Sleep(10000);
                    }
                }

            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}