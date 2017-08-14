using System;
using System.Collections.Generic;
using HotFix.Core;

namespace HotFix.Utilities
{
    public static class Scheduling
    {
        /// <summary>
        /// Returns the first session in the list that the given date matches, or null otherwise.
        /// </summary>
        /// <param name="sessions">A non-empty list of sessions.</param>
        /// <param name="at">The date and time to check against the sessions.</param>
        /// <returns>An active session, or null.</returns>
        public static ISchedule GetActive(this List<ISchedule> sessions, DateTime at)
        {
            foreach (var session in sessions)
            {
                if (session.IsActiveAt(at)) return session;
            }

            return null;
        }

        /// <summary>
        /// Returns the closing datetime of the session if it is active, or the closing datetime of the next occurence of the session.
        /// </summary>
        /// <param name="schedule">The session whose closing time needs to be calculated.</param>
        /// <param name="datetime">The date after which to calculate the closing time.</param>
        /// <returns>The next closing date and time.</returns>
        public static DateTime NextClosingTime(this ISchedule schedule, DateTime datetime)
        {
            var startOfWeek = (datetime - TimeSpan.FromDays((int)datetime.DayOfWeek)).Date;

            var startDate = startOfWeek.AddDays((int)schedule.OpenDay) + schedule.OpenTime;
            var endDate = startOfWeek.AddDays((int)schedule.CloseDay) + schedule.CloseTime;

            if (endDate > startDate) return datetime <= endDate ? endDate : endDate.AddDays(7);
            return datetime <= endDate ? endDate : endDate.AddDays(7);
        }

        /// <summary>
        /// Checks whether the datetime lies within the session.
        /// </summary>
        /// <param name="schedule">The session to check against.</param>
        /// <param name="datetime">The date and time to check against.</param>
        /// <returns>Whether the given datetime is in session.</returns>
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
