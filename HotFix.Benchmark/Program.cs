using System;
using System.IO;
using BenchmarkDotNet.Running;
using HotFix.Benchmark.Suites;

namespace HotFix.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Basic>();
            BenchmarkRunner.Run<Messages>();

            Directory.Delete(@"..\..\..\.bench", true);
            Directory.Move(@"BenchmarkDotNet.Artifacts\results", @"..\..\..\.bench");
        }
    }
}
