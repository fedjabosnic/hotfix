using System;
using BenchmarkDotNet.Running;
using Hotfix.Benchmark.Suites;

namespace Hotfix.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Basic>();
        }
    }
}
