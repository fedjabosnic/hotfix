using System;

namespace HotFix.Utilities
{
    /// <summary>
    /// An abstraction over time.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Returns the current universal time.
        /// </summary>
        DateTime Time { get; }
    }
}
