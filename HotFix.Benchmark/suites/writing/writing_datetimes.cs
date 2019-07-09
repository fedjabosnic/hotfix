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
    public class writing_datetimes
    {
        public DateTime DateTime;
        public byte[] Buffer;

        [GlobalSetup]
        public void Setup()
        {
            DateTime = DateTime.ParseExact("2017/03/27 15:45:13.596", "yyyy/MM/dd HH:mm:ss.fff", null);
            Buffer = new byte[21];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = DateTime.ToString("yyyyMMdd-HH:mm:ss.fff");
            return System.Text.Encoding.ASCII.GetBytes(s, 0, s.Length, Buffer, 0);
        }

        [Benchmark]
        public int hotfix() => Buffer.WriteDateTime(0, DateTime);
    }
}
