using System;
using static System.Globalization.DateTimeStyles;

namespace HotFix.Test
{
    public static class HelperExtensions
    {
        public static DateTime AsDateTime(this string date) => DateTime.ParseExact(date, "yyyy/MM/dd HH:mm:ss.fff", null, AssumeUniversal | AdjustToUniversal);

        public static byte[] AsBytes(this string text) => System.Text.Encoding.ASCII.GetBytes(text);
    }
}
