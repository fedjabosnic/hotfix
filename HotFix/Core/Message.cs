using System;

namespace HotFix.Core
{
    public class Message
    {
        public string Raw { get; private set; }
        public Field[] Fields { get; private set; }
        public int Count { get; private set; }

        public Message()
        {
            Fields =  new Field[1000];
        }

        public Message Parse(string message)
        {
            Raw = message;
            Count = 0;

            var start = 0;
            var tag = -1;

            for (var i = 0; i < message.Length; i++)
            {
                if (message[i] == '=' && tag == -1)
                {
                    tag = message.GetInt(start, i - start);

                    start = i + 1;
                }
                else if (message[i] == '\u0001')
                {
                    Fields[Count++] = new Field(message, tag, new Segment(start, i - start));

                    start = i + 1;
                    tag = -1;
                }
            }

            if (Fields[0].Tag !=  8) throw new Exception("BeginString field not found at expected position");
            if (Fields[1].Tag !=  9) throw new Exception("BodyLength field not found at expected position");
            if (Fields[2].Tag != 35) throw new Exception("MsgType field not found at expected position");
            if (Fields[Count - 1].Tag != 10) throw new Exception("CheckSum field not found at expected position");

            return this;
        }

        public Field this[int tag, int instance = 0]
        {
            get
            {
                switch (tag)
                {
                    case  8: return Fields[0];
                    case  9: return Fields[1];
                    case 35: return Fields[2];
                    case 10: return Fields[Count - 1];
                    default:
                        for (var i = 3; i < Count - 1; i++)
                            if (Fields[i].Tag == tag && --instance < 0)
                                return Fields[i];
                        throw new Exception("Field not found");
                }
            }
        }
    }
}