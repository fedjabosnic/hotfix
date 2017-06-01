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
    public class Floats
    {
        private int count = 1000000;

        [TestMethod]
        public void zero()
        {
            "0".GetFloat().Should().Be(0d);
        }

        [TestMethod]
        public void zero_with_decimals()
        {
            "0.0".GetFloat().Should().Be(0.0d);
        }

        [TestMethod]
        public void positive()
        {
            "123".GetFloat().Should().Be(123d);
        }

        [TestMethod]
        public void positive_with_decimals()
        {
            "123.456789".GetFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void negative()
        {
            "-123".GetFloat().Should().Be(-123d);
        }

        [TestMethod]
        public void negative_with_decimals()
        {
            "-123.456789".GetFloat().Should().Be(-123.456789d);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void performance_hotfix()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            var results = new List<double>(count);

            var throwaway1 = "1.23456789".GetFloat();
            var throwaway2 = "12.3456789".GetFloat();
            var throwaway3 = "123.456789".GetFloat();
            var throwaway4 = "1234.56789".GetFloat();
            var throwaway5 = "12345.6789".GetFloat();

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < count; i++)
            {
                results.Add("12345.6789".GetFloat());
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

            var throwaway1 = decimal.Parse("1.23456789");
            var throwaway2 = decimal.Parse("12.3456789");
            var throwaway3 = decimal.Parse("123.456789");
            var throwaway4 = decimal.Parse("1234.56789");
            var throwaway5 = decimal.Parse("12345.6789");

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < count; i++)
            {
                results.Add(decimal.Parse("12345.6789"));
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
