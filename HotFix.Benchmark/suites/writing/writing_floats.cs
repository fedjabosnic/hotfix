using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Encoding;

namespace HotFix.Benchmark.suites.writing
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000, id:"DAVE")]
    public class writing_floats
    {
        public double Number;
        public byte[] Buffer;

        [Setup]
        public void Setup()
        {
            Number = 123456789.123456;
            Buffer = new byte[16];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = Number.ToString();
            return System.Text.Encoding.ASCII.GetBytes(s, 0, s.Length, Buffer, 0);
        }

        [Benchmark]
        public int hotfix() => Buffer.WriteFloat(0, Number, 6);
    }
}
