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
        private string _string;
        private byte[] _buffer;

        [Setup]
        public void Setup()
        {
            _string = "random string";
            _buffer = new byte[13];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            return Encoding.ASCII.GetBytes(_string, 0, _string.Length, _buffer, 0);
        }

        [Benchmark]
        public int hotfix() => _buffer.WriteString(0, _string);
    }
}
