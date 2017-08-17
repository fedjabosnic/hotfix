using System;

namespace HotFix.Core
{
    public interface ISchedule
    {
        string Name { get; set; }
        DayOfWeek OpenDay { get; set; }
        TimeSpan OpenTime { get; set; }
        DayOfWeek CloseDay { get; set; }
        TimeSpan CloseTime { get; set; }
    }
}