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
    public class DateTimes
    {
        private int count = 1000000;

        [TestMethod]
        public void timestamp()
        {
            var result = "20170327 15:45:13".GetDateTime();

            result.Should().Be(DateTime.Parse("27/03/2017 15:45:13"));
        }

        [TestMethod]
        public void timestamp_with_milliseconds()
        {
            var result = "20170327 15:45:13.596".GetDateTime();

            result.Should().Be(DateTime.Parse("27/03/2017 15:45:13.596"));
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void performance_hotfix()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            var results = new List<DateTime>(count);

            var throwaway1 = "20170327 15:33:21.596".GetDateTime();
            var throwaway2 = "20170327 07:45:18.123".GetDateTime();
            var throwaway3 = "20170327 12:37:13.645".GetDateTime();
            var throwaway4 = "20170327 14:12:32.566".GetDateTime();
            var throwaway5 = "20170327 21:04:37.236".GetDateTime();

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < count; i++)
            {
                results.Add("20170327 15:45:13.596".GetDateTime());
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
            var results = new List<DateTime>(count);

            var throwaway1 = DateTime.ParseExact("20170327 15:33:21.596", "yyyyMMdd HH:mm:ss.fff", null);
            var throwaway2 = DateTime.ParseExact("20170327 07:45:18.123", "yyyyMMdd HH:mm:ss.fff", null);
            var throwaway3 = DateTime.ParseExact("20170327 12:37:13.645", "yyyyMMdd HH:mm:ss.fff", null);
            var throwaway4 = DateTime.ParseExact("20170327 14:12:32.566", "yyyyMMdd HH:mm:ss.fff", null);
            var throwaway5 = DateTime.ParseExact("20170327 21:04:37.236", "yyyyMMdd HH:mm:ss.fff", null);

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (int i = 0; i < count; i++)
            {
                results.Add(DateTime.ParseExact("20170327 15:45:13.596", "yyyyMMdd HH:mm:ss.fff", null));
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

    public static class Performance
    {
        
    }
}
