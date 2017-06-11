using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HotFix.Core
{
    public class Message
    {
        internal string Raw { get; private set; }
        internal Field[] Fields { get; private set; }
        public int Count { get; private set; }
        public bool Valid { get; private set; }

        public Message()
        {
            Fields = new Field[1000];
        }

        /// <summary>
        /// Parses a message from a string, expecting an entire message to be provided as a string. Any parsing errors will
        /// result in the message being invalid where the Valid property will return false and no fields will be accessible.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <returns>A reference to this instance, built from the parsed message.</returns>
        public Message Parse(string message)
        {
            try
            {
                Raw = message;
                Count = 0;

                var length = 0;
                var checksum = 0;

                for (var position = 0; position < message.Length; position++)
                {
                    var field = ParseField(message, ref position);

                    Fields[Count++] = field;

                    length += field.Length;
                    checksum += field.Checksum;
                }

                // Validate message (any errors at this point result in an invalid/garbled message)
                if (Fields[0].Tag !=  8) throw new Exception("BeginString field not found at expected position");
                if (Fields[1].Tag !=  9) throw new Exception("BodyLength field not found at expected position");
                if (Fields[2].Tag != 35) throw new Exception("MsgType field not found at expected position");
                if (Fields[Count - 1].Tag != 10) throw new Exception("CheckSum field not found at expected position");

                if (this[9].AsInt != length - this[8].Length - this[9].Length - this[10].Length) throw new Exception("BodyLength of the message does not match");
                if (this[10].AsInt != (checksum - this[10].Checksum) % 256) throw new Exception("CheckSum of the message does not match");

                Valid = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to parse message because {e.Message}");
                Valid = false;
                Count = 0;
            }

            return this;
        }

        /// <summary>
        /// Parses a fix field from the message from the provided position. Parsing will fail with an error
        /// if an illegal character is seen whilst parsing the tag or value.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="position">The position to parse from.</param>
        /// <returns>The parsed field.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Field ParseField(string message, ref int position)
        {
            var length = 0;
            var checksum = 0;

            // Parse next tag and value from the message
            var tag = ParseTag(message, ref position, ref length, ref checksum);
            var value = ParseValue(message, ref position, ref length, ref checksum);

            // Create the relevant field
            return new Field(message, tag, value, length, checksum);
        }

        /// <summary>
        /// Parses a fix tag from the message from the provided position. Parsing will fail with an error
        /// if an illegal character is seen before the tag is terminated.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="position">The position to parse from.</param>
        /// <param name="length">The current length of the whole field</param>
        /// <param name="checksum">The current checksum of the whole field</param>
        /// <returns>The tag number as an integer.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ParseTag(string message, ref int position, ref int length, ref int checksum)
        {
            var tag = 0;

            for (; position < message.Length; position++)
            {
                var b = message[position];

                length++;
                checksum += b;

                if (b == '=') break;
                if (b < '0' || b > '9') throw new Exception("Not a valid tag");

                tag *= 10;
                tag += b - '0';
            }

            position += 1;

            if (tag == 0) throw new Exception("Not a valid tag");
            return tag;
        }

        /// <summary>
        /// Parses a fix value from the message from the provided position. Parsing will fail with an error
        /// if an illegal character is seen before the value is terminated.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="position">The position to parse from.</param>
        /// <param name="length">The current length of the whole field</param>
        /// <param name="checksum">The current checksum of the whole field</param>
        /// <returns>The value as a segment of the original message.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Segment ParseValue(string message, ref int position, ref int length, ref int checksum)
        {
            var offset = position;

            for (; position < message.Length; position++)
            {
                var b = message[position];

                checksum += b;

                if (b == '\u0001') break;
            }

            var valueLength = position - offset;
            if (valueLength == 0) throw new Exception("Not a valid value");

            length += valueLength + 1;

            return new Segment(offset, valueLength);
        }

        /// <summary>
        /// Retrieves the field with the specified tag, optionally retrieving the specified instance (if there are groups).
        /// </summary>
        /// <param name="tag">The tag of the field to retrieve.</param>
        /// <param name="instance">The instance of the field (if there are multiple).</param>
        /// <returns>The field.</returns>
        public Field this[int tag, int instance = 0]
        {
            get
            {
                // NOTE: Optimized for retrieving expected header/trailer fields

                switch (tag)
                {
                    case  8: return Fields[0];
                    case  9: return Fields[1];
                    case 35: return Fields[2];
                    case 10: return Fields[Count - 1];
                    default:
                        // Linear search for relevant field (don't worry it's pretty fast)
                        for (var i = 3; i < Count - 1; i++)
                            if (Fields[i].Tag == tag && --instance < 0)
                                return Fields[i];
                        throw new Exception("Field not found");
                }
            }
        }

        public bool Contains(int tag, int instance = 0)
        {
            switch (tag)
            {
                case 8:
                case 9:
                case 35:
                case 10:
                    return true;
                default:
                    // Linear search for relevant field (don't worry it's pretty fast)
                    for (var i = 3; i < Count - 1; i++)
                        if (Fields[i].Tag == tag && --instance < 0)
                            return true;
                    return false;
            }
        }

        public override string ToString()
        {
            return Raw;
        }
    }
}