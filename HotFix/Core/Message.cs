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

            for (var current = 0; current < message.Length; current++)
            {
                // Parse next tag and value from the message
                var tag = ParseTag(message, ref current);
                var value = ParseValue(message, ref current);

                // Update the relevant field
                Fields[Count++] = new Field(message, tag, value);
            }

            // Validate message
            if (Fields[0].Tag !=  8) throw new Exception("BeginString field not found at expected position");
            if (Fields[1].Tag !=  9) throw new Exception("BodyLength field not found at expected position");
            if (Fields[2].Tag != 35) throw new Exception("MsgType field not found at expected position");
            if (Fields[Count - 1].Tag != 10) throw new Exception("CheckSum field not found at expected position");

            return this;
        }

        /// <summary>
        /// Parses a fix tag from the message from the provided position. Parsing will fail with an error
        /// if an illegal character is seen before the tag is terminated.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="position">The position to parse from.</param>
        /// <returns>The tag number as an integer.</returns>
        private int ParseTag(string message, ref int position)
        {
            var tag = 0;

            for (; position < message.Length; position++)
            {
                var b = message[position];

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
        /// <returns>The value as a segment of the original message.</returns>
        private Segment ParseValue(string message, ref int position)
        {
            var offset = position;
            var length = 0;

            for (; position < message.Length; position++)
            {
                var b = message[position];

                if (b == '\u0001') break;
                if (b == '=') throw new Exception("Not a valid value"); // NOTE: Legit?

                length++;
            }

            if (length == 0) throw new Exception("Not a valid value");

            return new Segment(offset, length);
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