using System;
using System.Text;
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
    public class writing_longs
    {
        private long _number;
        private byte[] _buffer;

        [Setup]
        public void Setup()
        {
            _number = 1234567890987654321;
            _buffer = new byte[19];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = _number.ToString();
            return System.Text.Encoding.ASCII.GetBytes(s, 0, s.Length, _buffer, 0);
        }

        [Benchmark]
        public int hotfix() => _buffer.WriteLong(0, _number);
    }
}
