using System;
using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using HotFix.Benchmark.suites;
using HotFix.Benchmark.suites.reading;
using HotFix.Benchmark.suites.session;
using HotFix.Benchmark.suites.writing;

namespace HotFix.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ManualConfig.Create(DefaultConfig.Instance).WithArtifactsPath(args[0]);

            BenchmarkRunner.Run<basic>(config);

            BenchmarkRunner.Run<reading_ints>(config);
            BenchmarkRunner.Run<reading_longs>(config);
            BenchmarkRunner.Run<reading_floats>(config);
            BenchmarkRunner.Run<reading_strings>(config);
            BenchmarkRunner.Run<reading_datetimes>(config);

            BenchmarkRunner.Run<writing_ints>(config);
            BenchmarkRunner.Run<writing_longs>(config);
            BenchmarkRunner.Run<writing_floats>(config);
            BenchmarkRunner.Run<writing_strings>(config);
            BenchmarkRunner.Run<writing_datetimes>(config);

            BenchmarkRunner.Run<parsing_messages>(config);
            BenchmarkRunner.Run<writing_messages>(config);

            BenchmarkRunner.Run<session_receive>(config);

            Console.ReadLine();
        }
    }
}
