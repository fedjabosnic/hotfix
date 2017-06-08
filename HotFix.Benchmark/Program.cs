using System;
using System.IO;
using BenchmarkDotNet.Running;

namespace HotFix.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Suites.Basic>();
            BenchmarkRunner.Run<Suites.Messages>();
            BenchmarkRunner.Run<Suites.Parsing.Ints>();
            BenchmarkRunner.Run<Suites.Parsing.Longs>();
            BenchmarkRunner.Run<Suites.Parsing.Floats>();
            BenchmarkRunner.Run<Suites.Parsing.DateTimes>();

            Directory.Delete(@"..\..\..\.bench", true);
            Directory.Move(@"BenchmarkDotNet.Artifacts\results", @"..\..\..\.bench");
        }
    }
}
