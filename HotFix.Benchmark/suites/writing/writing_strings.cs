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
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class writing_strings
    {
        public string String;
        public byte[] Buffer;

        [Setup]
        public void Setup()
        {
            String = "random string";
            Buffer = new byte[13];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            return System.Text.Encoding.ASCII.GetBytes(String, 0, String.Length, Buffer, 0);
        }

        [Benchmark]
        public int hotfix() => Buffer.WriteString(0, String);
    }
}
