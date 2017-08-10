using System;
using System.Collections.Generic;
using HotFix.Core;

namespace HotFix.Utilities
{
    public static class Scheduling
    {
        public static ISchedule GetActive(this List<ISchedule> sessions, DateTime at)
        {
            foreach (var session in sessions)
            {
                if (session.IsActiveAt(at)) return session;
            }

            return null;
        }

        public static DateTime NextClosingTime(this ISchedule schedule, DateTime datetime)
        {
            var startOfWeek = (datetime - TimeSpan.FromDays((int)datetime.DayOfWeek)).Date;

            return startOfWeek.AddDays((int)schedule.CloseDay) + schedule.CloseTime;
        }

        public static bool IsActiveAt(this ISchedule schedule, DateTime datetime)
        {
            var startOfWeek = (datetime - TimeSpan.FromDays((int)datetime.DayOfWeek)).Date;

            var startDate = startOfWeek.AddDays((int)schedule.OpenDay) + schedule.OpenTime;
            var endDate = startOfWeek.AddDays((int)schedule.CloseDay) + schedule.CloseTime;

            if (endDate > startDate)
            {
                if (startDate <= datetime && datetime <= endDate) return true;
            }
            else
            {
                if (datetime <= endDate || datetime >= startDate) return true;
            }

            return false;
        }
    }
}
