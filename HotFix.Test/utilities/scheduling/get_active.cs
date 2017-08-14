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
        private List<ISchedule> _schedules;

        [TestInitialize]
        public void Setup()
        {
            _schedules = new List<ISchedule>
            {
                new Schedule
                {
                    Name = "Session 1",
                    OpenDay = Friday,
                    OpenTime = TimeSpan.FromHours(10),
                    CloseDay = Friday,
                    CloseTime = TimeSpan.FromHours(11)
                },
                new Schedule
                {
                    Name = "Session 2",
                    OpenDay = Monday,
                    OpenTime = TimeSpan.FromHours(10),
                    CloseDay = Monday,
                    CloseTime = TimeSpan.FromHours(12)
                },
                new Schedule
                {
                    Name = "Session 3",
                    OpenDay = Monday,
                    OpenTime = TimeSpan.FromHours(11),
                    CloseDay = Monday,
                    CloseTime = TimeSpan.FromHours(13)
                }
            };
        }

        [TestMethod]
        public void returns_null_if_there_is_no_active_schedule()
        {
            var date = DateTime.ParseExact("11/08/2017 11:30:00.000", "dd/MM/yyyy HH:mm:ss.fff", null); // Friday 11:30
            var active = _schedules.GetActive(date);

            active.Should().Be(null);
        }

        [TestMethod]
        public void returns_the_active_schedule_if_there_is_one()
        {
            var date = DateTime.ParseExact("14/08/2017 10:30:00.000", "dd/MM/yyyy HH:mm:ss.fff", null); // Monday 10:30
            var active = _schedules.GetActive(date);

            active.Should().NotBe(null);
            active.Name.Should().Be("Session 2");
            active.OpenDay.Should().Be(Monday);
            active.OpenTime.Should().Be(TimeSpan.FromHours(10));
            active.CloseDay.Should().Be(Monday);
            active.CloseTime.Should().Be(TimeSpan.FromHours(12));
        }

        [TestMethod]
        public void returns_the_first_active_schedule_if_there_are_multiple()
        {
            var date = DateTime.ParseExact("14/08/2017 11:30:00.000", "dd/MM/yyyy HH:mm:ss.fff", null); // Monday 11:30
            var active = _schedules.GetActive(date);

            active.Should().NotBe(null);
            active.Name.Should().Be("Session 2");
            active.OpenDay.Should().Be(Monday);
            active.OpenTime.Should().Be(TimeSpan.FromHours(10));
            active.CloseDay.Should().Be(Monday);
            active.CloseTime.Should().Be(TimeSpan.FromHours(12));
        }
    }
}
