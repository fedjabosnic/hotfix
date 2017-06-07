using System;
using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Hotfix.Benchmark.Suites;

namespace Hotfix.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Basic>();
            BenchmarkRunner.Run<Messages>();

            Directory.Delete(@"..\..\..\.bench");
            Directory.Move(@"BenchmarkDotNet.Artifacts\results", @"..\..\..\.bench");
        }
    }
}
