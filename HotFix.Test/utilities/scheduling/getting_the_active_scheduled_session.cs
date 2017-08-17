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
    public class getting_the_active_scheduled_session
    {
        // August 2017 week 14th - 20th
        //
        // |14th |15th |16th |17th |18th |19th |20th |
        // |  M  |  T  |  W  |  T  |  F  |  S  |  S  |
        // -->|     |<--->|     |<--------->|     |<--
        //  ^     ^   ^     ^         ^   ^         ^ 
        //  A     B   C     D         E   F         G 
        //
        // The diagram above shows a week Monday - Sunday
        // There are three sessions:
        //  > Tuesday 10 am - Wednesday 10 am
        //  > Thursday 10 am - Saturday 10 am
        //  > Sunday 10 am - Monday 10 am (spans 2 weeks)
        // 
        // The scenarios A - G, as shown on the diagram
        // test the following cases:
        //  A: the part that overflows into the next week
        //  B: a period outside of a schedule
        //  C: first day of schedule
        //  D: period outside and between schedules
        //  E: middle day of schedule
        //  F: last day of schedule
        //  G: the current-week part in a 2-week schedule

        private List<Schedule> _schedules;

        [TestInitialize]
        public void Setup()
        {
            _schedules = new List<Schedule>
            {
                new Schedule
                {
                    Name = "Tuesday",
                    OpenDay = Tuesday,
                    OpenTime = TimeSpan.Parse("10:00:00"),
                    CloseDay = Wednesday,
                    CloseTime = TimeSpan.Parse("10:00:00")
                },
                new Schedule
                {
                    Name = "Thursday",
                    OpenDay = Thursday,
                    OpenTime = TimeSpan.Parse("10:00:00"),
                    CloseDay = Saturday,
                    CloseTime = TimeSpan.Parse("10:00:00")
                },
                new Schedule
                {
                    Name = "Sunday",
                    OpenDay = Sunday,
                    OpenTime = TimeSpan.Parse("10:00:00"),
                    CloseDay = Monday,
                    CloseTime = TimeSpan.Parse("10:00:00")
                }
            };
        }

        [TestMethod]
        public void scenario_a()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("14/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Sunday");
            schedule.Open.Should().Be(DateTime.ParseExact("13/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("14/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void scenario_b()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("15/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().Be(null);
        }

        [TestMethod]
        public void scenario_c()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("15/08/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Tuesday");
            schedule.Open.Should().Be(DateTime.ParseExact("15/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("16/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void scenario_d()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("16/08/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().Be(null);
        }

        [TestMethod]
        public void scenario_e()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("18/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Thursday");
            schedule.Open.Should().Be(DateTime.ParseExact("17/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("19/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void scenario_f()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("19/08/2017 08:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Thursday");
            schedule.Open.Should().Be(DateTime.ParseExact("17/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("19/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void scenario_g()
        {
            var schedule = _schedules.GetActive(DateTime.ParseExact("20/08/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));

            schedule.Should().NotBe(null);
            schedule.Name.Should().Be("Sunday");
            schedule.Open.Should().Be(DateTime.ParseExact("20/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
            schedule.Close.Should().Be(DateTime.ParseExact("21/08/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }
    }
}
