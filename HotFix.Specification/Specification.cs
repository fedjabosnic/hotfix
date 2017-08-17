using System;
using System.Collections.Generic;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification
{
    public class Specification
    {
        public Configuration Configuration { get; set; }
        public List<string> Instructions { get; set; }

        public Action<Session, Configuration, VirtualTransport> Verification { get; set; }

        public Type Exception { get; set; }
        public string Message { get; set; }

        public Specification Configure(Configuration configuration)
        {
            Configuration = configuration;

            return this;
        }

        public Specification Steps(List<string> instructions)
        {
            Instructions = instructions;

            return this;
        }

        public Specification Verify(Action<Session, Configuration, VirtualTransport> verification)
        {
            Verification = verification;

            return this;
        }

        public Specification Expect<T>(string message = null) where T : Exception
        {
            Exception = typeof(T);
            Message = message;

            return this;
        }

        public Specification Run()
        {
            var clock = new VirtualClock();
            var transport = new VirtualTransport(clock, Instructions);
            var engine = new Engine { Clocks = c => clock, Transports = c => transport };

            var session = (Session) null;

            try
            {
                using (session = engine.Open(Configuration))
                {
                    transport.Session = session;
                    session.Logon();

                    while (session.Active)
                    {
                        session.Receive();
                    }

                    session.Logout();
                }

                throw 
                    transport.Step == transport.Instructions.Count ? 
                    new DelightfullySuccessfulException() : 
                    new Exception("Session ended prematurely");
            }
            catch (Exception exception)
            {
                if (Exception != null)
                {
                    if (exception.GetType() == typeof(DelightfullySuccessfulException))
                        throw new AssertFailedException($"The test expected an exception of type '{Exception.FullName}' but no exception was thrown");

                    if (Exception != exception.GetType())
                        throw new AssertFailedException($"The test expected an exception of type '{Exception.FullName}' but an exception of type '{exception.GetType().FullName}' was thrown instead", exception);

                    if (Message != null && Message != exception.Message)
                        throw new AssertFailedException("The thrown exception did not have the correct message", exception);
                }
                else
                {
                    if (exception.GetType() != typeof(DelightfullySuccessfulException)) throw;

                    Verification.Invoke(session, Configuration, transport);
                }
            }
            finally
            {
                Console.WriteLine();

                for (var i = 0; i < Instructions.Count; i++)
                {
                    Console.WriteLine($"{i+1}: {Instructions[i]}");

                    if (i < transport.Step)
                        Console.WriteLine("   - SUCCEEDED");
                    if (i == transport.Step)
                        Console.WriteLine("   - FAILED");
                    if (i > transport.Step)
                        Console.WriteLine("   - SKIPPED");
                }
            }

            return this;
        }

    }
}