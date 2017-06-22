using System;
using System.IO;
using BenchmarkDotNet.Running;
using HotFix.Benchmark.suites;
using HotFix.Benchmark.suites.reading;
using HotFix.Benchmark.suites.writing;

namespace HotFix.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.Delete(@"..\..\..\.bench", true);

            BenchmarkRunner.Run<basic>();
            BenchmarkRunner.Run<parsing_messages>();
            BenchmarkRunner.Run<writing_messages>();
            BenchmarkRunner.Run<reading_ints>();
            BenchmarkRunner.Run<reading_longs>();
            BenchmarkRunner.Run<reading_floats>();
            BenchmarkRunner.Run<reading_strings>();
            BenchmarkRunner.Run<reading_datetimes>();
            BenchmarkRunner.Run<writing_ints>();
            BenchmarkRunner.Run<writing_longs>();
            BenchmarkRunner.Run<writing_floats>();
            BenchmarkRunner.Run<writing_strings>();
            BenchmarkRunner.Run<writing_datetimes>();

            Directory.Move(@"BenchmarkDotNet.Artifacts\results", @"..\..\..\.bench");
        }
    }
}
