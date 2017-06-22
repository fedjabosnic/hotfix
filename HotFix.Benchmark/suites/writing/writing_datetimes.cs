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
    public class writing_datetimes
    {
        private DateTime _dateTime;
        private byte[] _buffer;

        [Setup]
        public void Setup()
        {
            _dateTime = DateTime.Parse("2017/03/27 15:45:13.596");
            _buffer = new byte[21];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = _dateTime.ToString("yyyyMMdd-HH:mm:ss.fff");
            return Encoding.ASCII.GetBytes(s, 0, s.Length, _buffer, 0);
        }

        [Benchmark]
        public int hotfix() => _buffer.WriteDateTime(0, _dateTime);
    }
}
