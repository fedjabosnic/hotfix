using System;

namespace HotFix.Utilities
{
    /// <summary>
    /// An implementation of IClock which works off the actual system clock. If available, the time returned will be based on the high
    /// resolution clock within the operating system (GetSystemTimePreciseAsFileTime) - otherwise the default system time is used (System.DateTime).
    /// <remarks>
    /// You can check whether a high resolution clock is currently available via the <see cref="IsHighResolution"/> property.
    /// </remarks>
    /// </summary>
    public class RealTimeClock : IClock
    {
        /// <summary>
        /// Returns true if high resolution is available.
        /// </summary>
        public bool IsHighResolution { get; set; }

        public RealTimeClock()
        {
            try
            {
                long filetime;
                Os.GetSystemTimePreciseAsFileTime(out filetime);
                IsHighResolution = true;
            }
            catch (EntryPointNotFoundException)
            {
                IsHighResolution = false;
            }
        }

        /// <summary>
        /// Returns the current universal time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                if (!IsHighResolution) return DateTime.UtcNow;

                long time;
                Os.GetSystemTimePreciseAsFileTime(out time);
                return DateTime.FromFileTimeUtc(time);
            }
        }
    }
}