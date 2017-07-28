using System;
using System.Runtime.CompilerServices;

namespace HotFix.Encoding
{
    public static class FIXEncoding
    {
        /// <summary>
        /// Parses an integer (Int32) from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt(this byte[] source)
        {
            return source.ReadInt(0, source.Length);
        }

        /// <summary>
        /// Parses an integer (Int32) from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt(this byte[] source, int offset, int count)
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
        /// Parses a long (Int64) from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLong(this byte[] source)
        {
            return source.ReadLong(0, source.Length);
        }

        /// <summary>
        /// Parses a long (Int64) from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (5x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadLong(this byte[] source, int offset, int count)
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
        /// Parses an float (double) from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (3x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat(this byte[] source)
        {
            return source.ReadFloat(0, source.Length);
        }

        /// <summary>
        /// Parses an float (double) from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (3x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadFloat(this byte[] source, int offset, int count)
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
        /// Parses a datetime from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance implementation (allocates a new string).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(this byte[] source)
        {
            return source.ReadString(0, source.Length);
        }

        /// <summary>
        /// Parses a string from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance implementation (allocates a new string).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe string ReadString(this byte[] source, int offset, int count)
        {
            char* chars = stackalloc char[count + 1];

            for (var i = 0; i < count; i++)
            {
                *(chars + i) = (char)source[offset + i];
            }

            chars[count] = '\0';

            var s = new string(chars);

            return s;
        }

        /// <summary>
        /// Parses a datetime from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (10x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <returns>The parsed datetime.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ReadDateTime(this byte[] source)
        {
            return source.ReadDateTime(0, source.Length);
        }

        /// <summary>
        /// Parses a datetime from the provided byte array.
        /// </summary>
        /// <remarks>
        /// High performance, garbage free implementation (10x faster than bcl).
        /// </remarks>
        /// <param name="source">The source string to parse.</param>
        /// <param name="offset">The offset in the string to start parsing from.</param>
        /// <param name="count">The number of characters to parse.</param>
        /// <returns>The parsed datetime.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ReadDateTime(this byte[] source, int offset, int count)
        {
            if (count != 17 && count != 21) throw new Exception("Not a valid datetime");

            //if (source[ 8] != '-') throw new Exception("Not a valid datetime");
            //if (source[11] != ':') throw new Exception("Not a valid datetime");
            //if (source[14] != ':') throw new Exception("Not a valid datetime");
            //if (source[17] != '.') throw new Exception("Not a valid datetime");

            var year = source.ReadInt(offset + 00, 4);
            var month = source.ReadInt(offset + 04, 2);
            var day = source.ReadInt(offset + 06, 2);
            var hour = source.ReadInt(offset + 09, 2);
            var minute = source.ReadInt(offset + 12, 2);
            var second = source.ReadInt(offset + 15, 2);
            var millis = count == 21 ? source.ReadInt(offset + 18, 3) : 0;

            return new DateTime(year, month, day, hour, minute, second, millis);
        }

        /*
         * Notes:
         * 
         * There are a few ways of converting numbers to bytes, including:
         * 
         * Option 1:
         * 
         * Start from the left, printing the digits from the most to the least significant digit
         * 
         * In order to figure out what to print starting from the most significant either:
         * 
         *   a) Loop until you find the biggest divisor
         *   
         *   b) Calculate it by doing Floor(Log10(Abs(number)))
         * 
         * Option 2 (implemented):
         * 
         * Start from the right, printing the digits in reverse order, then reversing the bytes
         * 
         * Option 3:
         * 
         * Use the ToString() method, however this generates garbage
         * 
         * 
         * Option 1a seems to be faster than option 3 for integers, with better performance the smaller the number
         * Option 2 seems to be faster than options 1a and 3 for integers
         * 
         * Option 1a seems to be slower than option 3 for longs
         * Option 2 seems to be faster than options 1a and 3 for longs
         */

        /// <summary>
        /// Writes an integer to the given byte array.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position to start writing.</param>
        /// <param name="value">The number to write.</param>
        /// <returns>The number of characters written.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteInt(this byte[] buffer, int position, int value)
        {
            if (value == 0)
            {
                buffer[position] = (byte)'0';
                return 1;
            }

            var initial = position;
            var negative = false;

            if (value < 0)
            {
                negative = true;
                value = -value; // Note: Code won't work for int.MinValue
            }

            while (value > 0)
            {
                var n = '0' + (char)(value % 10);
                buffer[position++] = (byte)n;
                value /= 10;
            }

            if (negative) buffer[position++] = (byte)'-';

            for (var i = 0; i < (position - initial) / 2; i++)
            {
                var temp = buffer[initial + i];
                buffer[initial + i] = buffer[position - i - 1];
                buffer[position - i - 1] = temp;
            }

            return position - initial;
        }

        /// <summary>
        /// Writes an integer to the given byte array. 
        /// The number is written before the given position, so that the last digit is at that position.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position that marks the end of the number.</param>
        /// <param name="value">The number to write.</param>
        /// <returns>The number of characters written</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteIntBackwards(this byte[] buffer, int position, int value)
        {
            if (value == 0)
            {
                buffer[position] = (byte)'0';
                return 1;
            }

            var initial = position;
            var negative = false;

            if (value < 0)
            {
                negative = true;
                value = -value; // Note: Code won't work for int.MinValue
            }

            while (value > 0)
            {
                var n = '0' + (char)(value % 10);
                buffer[position--] = (byte)n;
                value /= 10;
            }

            if (negative) buffer[position--] = (byte)'-';

            return initial - position;
        }

        /// <summary>
        /// Writes a long to the given byte array.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position to start writing.</param>
        /// <param name="value">The number to write.</param>
        /// <returns>The number of characters written.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteLong(this byte[] buffer, int position, long value)
        {
            if (value == 0)
            {
                buffer[position] = (byte)'0';
                return 1;
            }

            var initial = position;
            var negative = false;

            if (value < 0)
            {
                negative = true;
                value = -value; // Note: Code won't work for long.MinValue
            }

            while (value > 0)
            {
                var n = '0' + (char)(value % 10);
                buffer[position++] = (byte)n;
                value /= 10;
            }

            if (negative) buffer[position++] = (byte)'-';

            for (var i = 0; i < (position - initial) / 2; i++)
            {
                var temp = buffer[initial + i];
                buffer[initial + i] = buffer[position - i - 1];
                buffer[position - i - 1] = temp;
            }

            return position - initial;
        }

        /// <summary>
        /// Writes a long to the given byte array. 
        /// The number is written before the given position, so that the last digit is at that position.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position that marks the end of the number.</param>
        /// <param name="value">The number to write.</param>
        /// <returns>The number of characters written</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteLongBackwards(this byte[] buffer, int position, long value)
        {
            if (value == 0)
            {
                buffer[position] = (byte)'0';
                return 1;
            }

            var initial = position;
            var negative = false;

            if (value < 0)
            {
                negative = true;
                value = -value; // Note: Code won't work for long.MinValue
            }

            while (value > 0)
            {
                var n = '0' + (char)(value % 10);
                buffer[position--] = (byte)n;
                value /= 10;
            }

            if (negative) buffer[position--] = (byte)'-';

            return initial - position;
        }

        /// <summary>
        /// Writes a double to the given byte array. 
        /// The number is rounded to 6 decimal places.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position to start writing.</param>
        /// <param name="value">The number to write.</param>
        /// <returns>The number of characters written.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteFloat(this byte[] buffer, int position, double value)
        {
            var start = position;
            var integer = (long)value;
            var fraction = (long)Math.Round(Math.Abs(value) * 1000000L) % 1000000L;

            position += buffer.WriteLong(position, integer);
            buffer[position++] = (byte)'.';
            position += buffer.WriteLong(position, fraction);

            return position - start;
        }

        /// <summary>
        /// Writes a string to the given byte array.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position to start writing.</param>
        /// <param name="value">The string to write.</param>
        /// <returns>The number of characters written.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString(this byte[] buffer, int position, string value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                buffer[position + i] = (byte)value[i];
            }

            return value.Length;
        }

        /// <summary>
        /// Writes a date and time to the given byte array.
        /// </summary>
        /// <param name="buffer">The buffer to write to.</param>
        /// <param name="position">The position to start writing.</param>
        /// <param name="value">The date and time to write.</param>
        /// <returns>The number of characters written.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteDateTime(this byte[] buffer, int position, DateTime value)
        {
            var year = value.Year;
            buffer[position] = (byte)('0' + year / 1000);
            year %= 1000;
            buffer[position + 1] = (byte)('0' + year / 100);
            year %= 100;
            buffer[position + 2] = (byte)('0' + year / 10);
            year %= 10;
            buffer[position + 3] = (byte)('0' + year);

            var month = value.Month;
            buffer[position + 4] = (byte)('0' + month / 10);
            month %= 10;
            buffer[position + 5] = (byte)('0' + month);

            var day = value.Day;
            buffer[position + 6] = (byte)('0' + day / 10);
            day %= 10;
            buffer[position + 7] = (byte)('0' + day);

            buffer[position + 8] = (byte)'-';

            var hour = value.Hour;
            buffer[position + 9] = (byte)('0' + hour / 10);
            hour %= 10;
            buffer[position + 10] = (byte)('0' + hour);

            buffer[position + 11] = (byte)':';

            var minute = value.Minute;
            buffer[position + 12] = (byte)('0' + minute / 10);
            minute %= 10;
            buffer[position + 13] = (byte)('0' + minute);

            buffer[position + 14] = (byte)':';

            var second = value.Second;
            buffer[position + 15] = (byte)('0' + second / 10);
            second %= 10;
            buffer[position + 16] = (byte)('0' + second);

            buffer[position + 17] = (byte)'.';

            var millis = value.Millisecond;
            buffer[position + 18] = (byte)('0' + millis / 100);
            millis %= 100;
            buffer[position + 19] = (byte)('0' + millis / 10);
            millis %= 10;
            buffer[position + 20] = (byte)('0' + millis);

            return 21;
        }
    }
}
