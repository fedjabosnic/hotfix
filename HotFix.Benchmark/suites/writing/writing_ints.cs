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
    public class writing_ints
    {
        private int _number;
        private byte[] _buffer;

        [Setup]
        public void Setup()
        {
            _number = 1234567890;
            _buffer = new byte[10];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = _number.ToString();
            return Encoding.ASCII.GetBytes(s, 0, s.Length, _buffer, 0);
        }

        [Benchmark]
        public int hotfix() => _buffer.WriteInt(0, _number);
    }
}