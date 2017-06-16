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
    public class writing_strings
    {
        public string String { get; set; }
        public byte[] Buffer { get; set; }

        [Setup]
        public void Setup()
        {
            String = "random string";
            Buffer = new byte[13];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            return Encoding.ASCII.GetBytes(String, 0, String.Length, Buffer, 0);
        }

        [Benchmark]
        public int hotfix() => Buffer.WriteString(0, String);
    }
}
