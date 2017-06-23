using System;
using System.Collections.Generic;
using HotFix.Core;
using HotFix.Transport;

namespace HotFix.Specification
{
    public class Harness : ITransport
    {
        public VirtualClock Clock { get; set; }
        public Queue<string> Instructions { get; private set; }
        public string Outbound { get; private set; }

        public Harness(Queue<string> instructions)
        {
            Instructions = instructions;
            Engine.Clock = Clock = new VirtualClock();

            var instruction = Instructions.Dequeue();

            Clock.Time = DateTime.ParseExact(instruction.Substring(2, instruction.Length - 2), "yyyyMMdd-HH:mm:ss.fff", null);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (Instructions.Count == 0) throw new DelightfullySuccessfulException();

            var instruction = Instructions.Dequeue();

            switch (instruction[0])
            {
                case '!':
                    Clock.Time = DateTime.ParseExact(instruction.Substring(2, instruction.Length - 2), "yyyyMMdd-HH:mm:ss.fff", null);
                    return 0;
                case '<':
                    System.Text.Encoding.UTF8.GetBytes(instruction, 2, instruction.Length - 2, buffer, offset);
                    return instruction.Length - 2;
                case '>':
                    throw new Exception("Outbound message expected");
                default:
                    throw new Exception("Blaah");
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var instruction = Instructions.Dequeue();
            var outbound = System.Text.Encoding.UTF8.GetString(buffer, offset, count);

            if (instruction[0] != '>') throw new Exception("Outbound message not expected");

            var expected = instruction.Substring(2, instruction.Length - 2);

            if (expected != outbound)
            {
                throw new Exception("Outbound message not correct");
            }
        }

        public void Dispose()
        {

        }
    }
}