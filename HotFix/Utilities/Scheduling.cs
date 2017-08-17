using System;
using System.Collections.Generic;
using HotFix.Core;

namespace HotFix.Utilities
{
    public static class Scheduling
    {
        /// <summary>
        /// Returns the open and close dates and times of the first schedule in the list that the given date matches, or null otherwise.
        /// </summary>
        /// <param name="schedules">A non-empty list of schedules.</param>
        /// <param name="at">The date and time to check against the schedules.</param>
        /// <returns>The active schedule, or null.</returns>
        internal static ActiveSchedule GetActive(this List<Schedule> schedules, DateTime at)
        {
            foreach (var schedule in schedules)
            {
                var name = schedule.Name;

                var startOfWeek = (at - TimeSpan.FromDays((int)at.DayOfWeek)).Date;

                var openDate = startOfWeek.AddDays((int)schedule.OpenDay) + schedule.OpenTime;
                var closeDate = startOfWeek.AddDays((int)schedule.CloseDay) + schedule.CloseTime;

                if (closeDate > openDate)
                {
                    if (openDate <= at && at <= closeDate) return new ActiveSchedule { Name = name, Open = openDate, Close = closeDate};
                }
                else
                {
                    if (at <= closeDate) return new ActiveSchedule { Name = name, Open = openDate, Close = closeDate};
                    if (at >= openDate) return new ActiveSchedule { Name = name, Open = openDate, Close = closeDate.AddDays(7)};
                }
            }

            return null;
        }
    }
}
