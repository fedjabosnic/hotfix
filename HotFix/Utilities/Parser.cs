﻿using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace HotFix.Utilities
{
    public static class Parser
    {
        /// <summary>
        /// Appends the given character to the string builder and returns true if it now forms a
        /// complete fix message.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="c">The additional character.</param>
        /// <returns>True if the builder contains a complete fix message, false otherwise.</returns>
        public static bool Build(this StringBuilder builder, char c)
        {
            builder.Append(c);

            if (c != '\u0001') return false;

            var length = builder.Length;

            return length > 8 &&
                   builder[length - 8] == '\u0001' &&
                   builder[length - 7] == '1' &&
                   builder[length - 6] == '0' &&
                   builder[length - 5] == '=';
        }

        /// <summary>
        /// Parses an integer (Int32) from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInt(this string source)
        {
            return source.GetInt(0, source.Length);
        }

        /// <summary>
        /// Parses an integer (Int32) from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInt(this string source, int offset, int count)
        {
            var value = 0;
            var sign = source[offset] == '-' ? -1 : +1;

            var skip = sign < 0 ? 1 : 0;

            for (var i = 0; i < count - skip; i++)
            {
                var b = source[offset + skip + i];

                if (b < '0' || b > '9') throw new Exception("Not a valid int");

                value *= 10;
                value += b - '0';
            }

            return value * sign;
        }

        /// <summary>
        /// Parses a long (Int64) from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetLong(this string source)
        {
            return source.GetLong(0, source.Length);
        }

        /// <summary>
        /// Parses a long (Int64) from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetLong(this string source, int offset, int count)
        {
            var value = 0L;
            var sign = source[offset] == '-' ? -1 : +1;

            var skip = sign < 0 ? 1 : 0;

            for (var i = 0; i < count - skip; i++)
            {
                var b = source[offset + skip + i];

                if (b < '0' || b > '9') throw new Exception("Not a valid int");

                value *= 10;
                value += b - '0';
            }

            return value * sign;
        }

        /// <summary>
        /// Parses an float (double) from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (3x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetFloat(this string source)
        {
            return source.GetFloat(0, source.Length);
        }

        /// <summary>
        /// Parses an float (double) from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (3x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetFloat(this string source, int offset, int count)
        {
            var value = 0L;
            var exponent = 0d;
            var sign = source[offset] == '-' ? -1 : +1;

            var skip = sign < 0 ? 1 : 0;

            for (var i = 0; i < count - skip; i++)
            {
                var b = source[offset + skip + i];

                if (b < '0' || b > '9')
                {
                    if (b != '.') throw new Exception("Not a valid float");

                    exponent = 1d;
                    continue;
                }

                value *= 10;
                value += b - '0';
                exponent *= 10;
            }

            if (exponent == 0d) exponent = 1d;

            return sign * (value / exponent);
        }

        /// <summary>
        /// Parses a datetime from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (10x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed datetime.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime GetDateTime(this string source)
        {
            return source.GetDateTime(0, source.Length);
        }

        /// <summary>
        /// Parses a datetime from the provided string.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (10x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed datetime.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime GetDateTime(this string source, int offset, int count)
        {
            if (count != 17 && count != 21) throw new Exception("Not a valid datetime");

            //if (source[ 8] != '-') throw new Exception("Not a valid datetime");
            //if (source[11] != ':') throw new Exception("Not a valid datetime");
            //if (source[14] != ':') throw new Exception("Not a valid datetime");
            //if (source[17] != '.') throw new Exception("Not a valid datetime");

            var year = source.GetInt(offset + 00, 4);
            var month = source.GetInt(offset + 04, 2);
            var day = source.GetInt(offset + 06, 2);
            var hour = source.GetInt(offset + 09, 2);
            var minute = source.GetInt(offset + 12, 2);
            var second = source.GetInt(offset + 15, 2);
            var millis = count == 21 ? source.GetInt(offset + 18, 3) : 0;

            return new DateTime(year, month, day, hour, minute, second, millis);
        }
    }
}
