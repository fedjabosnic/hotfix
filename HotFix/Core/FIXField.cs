using System;
using HotFix.Encoding;

namespace HotFix.Core
{
    public struct FIXField
    {
        private readonly byte[] _message;
        private readonly Segment _value;

        public int Tag { get; }
        public int Length { get; }
        public int Checksum { get; }

        public int AsInt => _message.ReadInt(_value.Offset, _value.Length);
        public long AsLong => _message.ReadLong(_value.Offset, _value.Length);
        public double AsFloat => _message.ReadFloat(_value.Offset, _value.Length);
        public string AsString => _message.ReadString(_value.Offset, _value.Length);
        public DateTime AsDateTime => _message.ReadDateTime(_value.Offset, _value.Length);

        public FIXField(byte[] message, int tag, Segment value, int length, int checksum)
        {
            _message = message;
            _value = value;

            Tag = tag;
            Length = length;
            Checksum = checksum;
        }

        /// <summary>
        /// Returns true if the field value is equal to the specified int, false if the value
        /// is not a valid int or it isn't equal to the specified value.
        /// <remarks>
        /// This operation is garbage free.
        /// </remarks>
        /// </summary>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The result of the comparison.</returns>
        public bool Is(int value)
        {
            try
            {
                return this.AsInt == value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the field value is equal to the specified long, false if the value
        /// is not a valid long or it isn't equal to the specified value.
        /// <remarks>
        /// This operation is garbage free.
        /// </remarks>
        /// </summary>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The result of the comparison.</returns>
        public bool Is(long value)
        {
            try
            {
                return this.AsLong == value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the field value is equal to the specified double, false if the value
        /// is not a valid double or it isn't equal to the specified value.
        /// <remarks>
        /// This operation is garbage free.
        /// </remarks>
        /// </summary>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The result of the comparison.</returns>
        public bool Is(double value)
        {
            try
            {
                return this.AsFloat == value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the field value is equal to the specified string, false if the value
        /// is not a valid string or it isn't equal to the specified value.
        /// <remarks>
        /// This operation is garbage free.
        /// </remarks>
        /// </summary>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The result of the comparison.</returns>
        public bool Is(string value)
        {
            if (_value.Length != value.Length) return false;

            for (var i = 0; i < _value.Length; i++)
            {
                if (_message[_value.Offset + i] != value[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the field value is equal to the specified datetime, false if the value
        /// is not a valid datetime or it isn't equal to the specified value.
        /// <remarks>
        /// This operation is garbage free.
        /// </remarks>
        /// </summary>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The result of the comparison.</returns>
        public bool Is(DateTime value)
        {
            try
            {
                return this.AsDateTime == value;
            }
            catch
            {
                return false;
            }
        }
    }
}