using System;

namespace HotFix.Core
{
    public class Schedule : ISchedule
    {
        public string Name { get; set; }
        public DayOfWeek OpenDay { get; set; }
        public TimeSpan OpenTime { get; set; }
        public DayOfWeek CloseDay { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}