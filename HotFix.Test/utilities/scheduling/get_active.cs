using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.DayOfWeek;

namespace HotFix.Test.utilities.scheduling
{
    [TestClass]
    public class get_active
    {
        // |  M  |  T  |  W  |  T  |  F  |  S  |  S  |
        // -->|     |<--->|     |<--------->|     |<--
        //  ^    ^     ^     ^        ^   ^         ^ 
        //  A    B     C     D        E   F         G 

        private List<ISchedule> _schedules;

        [TestInitialize]
        public void Setup()
        {
            _schedules = new List<ISchedule>
            {
                new Schedule
                {
                    Name = "Tuesday",
                    OpenDay = Tuesday,
                    OpenTime = TimeSpan.FromHours(10),
                    CloseDay = Wednesday,
                    CloseTime = TimeSpan.FromHours(10)
                },
                new Schedule
                {
                    Name = "Thursday",
                    OpenDay = Thursday,
                    OpenTime = TimeSpan.FromHours(10),
                    CloseDay = Saturday,
                    CloseTime = TimeSpan.FromHours(10)
                },
                new Schedule
                {
                    Name = "Sunday",
                    OpenDay = Sunday,
                    OpenTime = TimeSpan.FromHours(10),
                    CloseDay = Monday,
                    CloseTime = TimeSpan.FromHours(10)
                }
            };
        }

        [TestMethod]
        public void test_a()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("14/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Sunday");
            schedule.Open.Should().Be(DateTime.ParseExact("13/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("14/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test_b()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("15/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().Be(null);
        }

        [TestMethod]
        public void test_c()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("16/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Tuesday");
            schedule.Open.Should().Be(DateTime.ParseExact("15/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("16/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test_d()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("17/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().Be(null);
        }

        [TestMethod]
        public void test_e()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("18/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Thursday");
            schedule.Open.Should().Be(DateTime.ParseExact("17/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("19/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test_f()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("19/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Thursday");
            schedule.Open.Should().Be(DateTime.ParseExact("17/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("19/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test_g()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("20/08/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Sunday");
            schedule.Open.Should().Be(DateTime.ParseExact("20/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("21/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }
    }
}
