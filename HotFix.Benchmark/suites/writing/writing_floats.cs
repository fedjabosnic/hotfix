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
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000, id:"DAVE")]
    public class writing_floats
    {
        private double _number;
        private byte[] _buffer;

        [Setup]
        public void Setup()
        {
            _number = 123456789.123456;
            _buffer = new byte[16];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = _number.ToString();
            return Encoding.ASCII.GetBytes(s, 0, s.Length, _buffer, 0);
        }

        [Benchmark]
        public int hotfix() => _buffer.WriteFloat(0, _number);
    }
}
