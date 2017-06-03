using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HotFix.Transport;

namespace HotFix.Core
{
    public class Session : IDisposable
    {
        public ITransport Transport { get; }
        public IConfiguration Configuration { get; }

        public Message InboundMessage { get; } = new Message();

        public event Action<Message> Inbound;

        public Session(ITransport transport, IConfiguration configuration)
        {
            Transport = transport;
            Configuration = configuration;
        }

        public void Run()
        {
            var buffer = new byte[Configuration.InboundBufferSize];
            var builder = new StringBuilder(Configuration.InboundBufferSize);

            // Logon
            Send("A", $"98=0|108={Configuration.HeartbeatInterval}|141=Y|");

            var tail = 0;
            var head = 0;
            var heartbeat = DateTime.UtcNow;

            while (true)
            {
                if (DateTime.UtcNow - heartbeat > TimeSpan.FromSeconds(Configuration.HeartbeatInterval))
                {
                    Send("0", "");

                    heartbeat = DateTime.UtcNow;
                }

                try
                {
                    var read = Transport.Inbound.Read(buffer, head, buffer.Length - head);

                    head += read;

                    for (; tail < head; tail++)
                    {
                        var c = (char)buffer[tail];

                        builder.Append(c);

                        if (c == '\u0001')
                        {
                            var l = builder.Length - 1;

                            if (l > 7 && builder[l - 7] == '\u0001' && builder[l - 6] == '1' && builder[l - 5] == '0' && builder[l - 4] == '=')
                            {
                                try
                                {
                                    // Parse message
                                    InboundMessage.Parse(builder.ToString());

                                    // Process message
                                    Process(InboundMessage);
                                }
                                finally
                                {
                                    builder.Clear();
                                }
                            }
                        }
                    }

                    if (tail == head)
                    {
                        tail = 0;
                        head = 0;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"! {e.Message}");
                }
            }
        }

        private void Process(Message message)
        {
            switch (message[35].String)
            {
                case "0":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Heartbeat");
                    break;
                case "A":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Logon");
                    break;
                case "5":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Logout");
                    break;
                case "3":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Reject");
                    break;
                case "2":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Resend request");
                    break;
                case "4":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Sequence reset");
                    break;
                case "1":
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Test request");
                    break;
                default:
                    Debug.WriteLine($"< {message[52].DateTime:yyyyMMdd HH:mm:ss.fff}: Application message ({message[35].String})");
                    break;
            }

            Inbound?.Invoke(message);
        }

        public void Send(string type, string bodypart)
        {
            // NOTE: This is temporary until we implement an efficient message builder

            var version = Configuration.Version;
            var sender = Configuration.Sender;
            var target = Configuration.Target;
            var seqnum = Configuration.OutboundSeqNum++;
            var timestamp = DateTime.UtcNow;

            var body = $"35={type}|34={seqnum}|49={sender}|52={timestamp:yyyyMMdd-HH:mm:ss.fff}|56={target}|{bodypart}".Replace("|", "\u0001");
            var header = $"8={version}|9={body.Length}|".Replace("|", "\u0001");
            var trailer = $"10={((header + body).ToCharArray().Select(x => (int)x).Sum() % 256):D3}|".Replace("|", "\u0001");

            var message = header + body + trailer;
            var msg = Encoding.UTF8.GetBytes(message);

            // TODO: Do we want to decrement the sequence number if creating/sending the message fails?

            Debug.WriteLine($"> {timestamp:yyyyMMdd HH:mm:ss.fff}: Sending '{type}'");
            Debug.WriteLine($"  {message}");

            Transport.Outbound.Write(msg, 0, msg.Length);
        }

        public void Dispose()
        {
            Transport?.Dispose();
        }
    }
}
