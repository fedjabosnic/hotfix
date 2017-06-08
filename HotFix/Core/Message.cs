using System;

namespace HotFix.Core
{
    public class Message
    {
        internal string Raw { get; private set; }
        internal Field[] Fields { get; private set; }
        public int Count { get; private set; }

        public Message()
        {
            Fields = new Field[1000];
        }

        /// <summary>
        /// Parses a message from a string, expecting the entire message in one go.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <returns>A reference to this instance, built from the parsed message.</returns>
        public Message Parse(string message)
        {
            Raw = message;
            Count = 0;

            var totalLength = 0;
            var totalChecksum = 0;

            for (var current = 0; current < message.Length; current++)
            {
                var checksum = 0;
                var length = 0;

                // Parse next tag and value from the message
                var tag = ParseTag(message, ref current, ref length, ref checksum);
                var value = ParseValue(message, ref current, ref length, ref checksum);

                // Update the relevant field
                Fields[Count++] = new Field(message, tag, length, checksum, value);

                totalLength += length;
                totalChecksum += checksum;
            }

            // Validate message
            if (Fields[0].Tag !=  8) throw new Exception("BeginString field not found at expected position");
            if (Fields[1].Tag !=  9) throw new Exception("BodyLength field not found at expected position");
            if (Fields[2].Tag != 35) throw new Exception("MsgType field not found at expected position");
            if (Fields[Count - 1].Tag != 10) throw new Exception("CheckSum field not found at expected position");

            var beginString = Fields[0];
            var bodyLength = Fields[1];
            var checkSum = Fields[Count - 1];
            if (totalLength - beginString.Length - bodyLength.Length - checkSum.Length != bodyLength.Int) throw new Exception("BodyLength of the message does not match");
            if ((totalChecksum - checkSum.Checksum) % 256 != checkSum.Int) throw new Exception("CheckSum of the message does not match");

            return this;
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
        private Segment ParseValue(string message, ref int position, ref int length, ref int checksum)
        {
            var offset = position;

            for (; position < message.Length; position++)
            {
                var b = message[position];

                checksum += b;

                if (b == '\u0001') break;
                if (b == '=') throw new Exception("Not a valid value"); // TODO: Check whether this is legit?
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

        public override string ToString()
        {
            return Raw;
        }
    }
}