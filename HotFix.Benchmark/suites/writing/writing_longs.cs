using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Utilities;

namespace HotFix.Benchmark.suites.writing
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class writing_longs
    {
        public long Number { get; set; }
        public byte[] Buffer { get; set; }

        [Setup]
        public void Setup()
        {
            Number = 1234567890987654321;
            Buffer = new byte[19];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = Number.ToString();
            return Encoding.ASCII.GetBytes(s, 0, s.Length, Buffer, 0);
        }

        [Benchmark]
        public int hotfix() => Buffer.WriteLong(0, Number);
    }
}
