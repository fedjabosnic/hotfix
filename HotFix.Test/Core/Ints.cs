using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Threading;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.Core
{
    [TestClass]
    public class Ints
    {
        private int count = 1000000;

        [TestMethod]
        public void zero()
        {
            "0".GetInt().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            "123".GetInt().Should().Be(123);
        }

        [TestMethod]
        public void negative()
        {
            "-123".GetInt().Should().Be(-123);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void performance_hotfix()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            var results = new List<double>(count);

            var throwaway1 = "12345".GetInt();
            var throwaway2 = "23456".GetInt();
            var throwaway3 = "34567".GetInt();
            var throwaway4 = "45678".GetInt();
            var throwaway5 = "56789".GetInt();

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < count; i++)
            {
                results.Add("12345".GetInt());
            }

            timer.Stop();

            Console.WriteLine($"Entries: {count}");
            Console.WriteLine($"Elapsed: {timer.ElapsedMilliseconds} millis");
            Console.WriteLine($"Rates:   {count / ((double)timer.ElapsedTicks / Stopwatch.Frequency)} entries per second");
            Console.WriteLine($"         {((double)timer.ElapsedTicks / Stopwatch.Frequency) / count * 1000000} micros per entry");

            Console.WriteLine($"Collections:");
            Console.WriteLine($"      Gen 0: {GC.CollectionCount(0) - c0}");
            Console.WriteLine($"      Gen 1: {GC.CollectionCount(1) - c1}");
            Console.WriteLine($"      Gen 2: {GC.CollectionCount(2) - c2}");

            Console.WriteLine();
            Console.WriteLine(throwaway1);
            Console.WriteLine(throwaway2);
            Console.WriteLine(throwaway3);
            Console.WriteLine(throwaway4);
            Console.WriteLine(throwaway5);

            Console.WriteLine(results[12345]);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void performance_standard()
        {
            var results = new List<decimal>(count);

            var throwaway1 = int.Parse("12345");
            var throwaway2 = int.Parse("23456");
            var throwaway3 = int.Parse("34567");
            var throwaway4 = int.Parse("45678");
            var throwaway5 = int.Parse("56789");

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < count; i++)
            {
                results.Add(int.Parse("12345"));
            }

            timer.Stop();

            Console.WriteLine($"Entries: {count}");
            Console.WriteLine($"Elapsed: {timer.ElapsedMilliseconds} millis");
            Console.WriteLine($"Rates:   {count / ((double)timer.ElapsedTicks / Stopwatch.Frequency)} entries per second");
            Console.WriteLine($"         {((double)timer.ElapsedTicks / Stopwatch.Frequency) / count * 1000000} micros per entry");

            Console.WriteLine($"Collections:");
            Console.WriteLine($"      Gen 0: {GC.CollectionCount(0) - c0}");
            Console.WriteLine($"      Gen 1: {GC.CollectionCount(1) - c1}");
            Console.WriteLine($"      Gen 2: {GC.CollectionCount(2) - c2}");

            Console.WriteLine();
            Console.WriteLine(throwaway1);
            Console.WriteLine(throwaway2);
            Console.WriteLine(throwaway3);
            Console.WriteLine(throwaway4);
            Console.WriteLine(throwaway5);

            Console.WriteLine(results[12345]);
        }
    }
}