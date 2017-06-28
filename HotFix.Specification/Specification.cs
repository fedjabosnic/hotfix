using System;
using System.Collections.Generic;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification
{
    public class Specification
    {
        public IConfiguration Configuration { get; set; }
        public List<string> Instructions { get; set; }

        public Action<Engine, IConfiguration> Verification { get; set; }

        public Type Exception { get; set; }
        public string Message { get; set; }

        public Specification Configure(IConfiguration configuration)
        {
            Configuration = configuration;

            return this;
        }

        public Specification Steps(List<string> instructions)
        {
            Instructions = instructions;

            return this;
        }

        public Specification Verify(Action<Engine, IConfiguration> verification)
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
            var harness = (Harness)null;
            var engine = new Engine { Transports = c => harness = new Harness(Instructions) };

            try
            {
                engine.Run(Configuration);

                if (Exception != null) throw new AssertFailedException("The expected exception was not thrown");
            }
            catch (Exception exception)
            {
                if (Exception != null)
                {
                    if (exception.GetType() == typeof(DelightfullySuccessfulException))
                        throw new AssertFailedException("The expected exception type was not thrwon. All instructions successfully executed instead");

                    if (Exception != exception.GetType())
                        throw new AssertFailedException($"The expected exception type was not thrown. The type '{exception.GetType()}' was thrown instead");

                    if (Message != null && Message != exception.Message)
                        throw new AssertFailedException("The thrown exception did not have the correct message");
                }
                else
                {
                    if (exception.GetType() != typeof(DelightfullySuccessfulException)) throw;

                    Verification.Invoke(engine, Configuration);
                }
            }
            finally
            {
                Console.WriteLine();

                for (var i = 1; i <= Instructions.Count; i++)
                {
                    Console.WriteLine($"{i}: {Instructions[i - 1]}");

                    if (i < harness.Step)
                        Console.WriteLine("   - SUCCEEDED");
                    if (i == harness.Step)
                        Console.WriteLine("   - FAILED");
                    if (i > harness.Step)
                        Console.WriteLine("   - SKIPPED");
                }
            }

            return this;
        }
    }
}