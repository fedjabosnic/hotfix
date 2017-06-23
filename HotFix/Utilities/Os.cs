using System;
using System.Runtime.InteropServices;

namespace HotFix.Utilities
{
    internal static class Os
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        internal static extern void GetSystemTimePreciseAsFileTime(out long filetime);
    }
}