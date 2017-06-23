using System;
using HotFix.Utilities;

namespace HotFix.Specification
{
    /// <summary>
    /// A virtual clock that allows the current time to be set.
    /// </summary>
    public class VirtualClock : IClock
    {
        public DateTime Time { get; set; }
    }
}