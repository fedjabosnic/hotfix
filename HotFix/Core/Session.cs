using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HotFix.Transport;
using HotFix.Utilities;

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
            var message = new StringBuilder(Configuration.InboundBufferSize);

            // Logon
            Send("A", $"98=0|108={Configuration.HeartbeatInterval}|141=Y|");

            var tail = 0;
            var head = 0;

            while (true)
            {
                try
                {
                    head += Transport.Inbound.Read(buffer, head, buffer.Length - head);

                    for (; tail < head; tail++)
                    {
                        if (message.Build((char) buffer[tail]))
                        {
                            try
                            {
                                // Parse message
                                InboundMessage.Parse(message.ToString());

                                // Process message
                                Process(InboundMessage);
                            }
                            finally
                            {
                                message.Clear();
                            }
                        }
                    }

                    if (tail == head) tail = head = 0;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"! {e.Message}");
                }

                // Send heartbeats
                if (DateTime.UtcNow - Configuration.OutboundUpdatedAt > TimeSpan.FromSeconds(Configuration.HeartbeatInterval)) Send("0", "");
            }
        }

        private void Process(Message message)
        {
            var seqnum = message[34].Long;
            var expected = Configuration.InboundSeqNum + 1;

            if (seqnum > expected)
            {
                SendResendRequest();
                return;
            }

            if (seqnum < expected)
            {
                throw new Exception("Inbound sequence number too low");
            }

            Configuration.Synchronised = true;

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

            Configuration.InboundSeqNum++;
        }

        public void SendResendRequest()
        {
            if (!Configuration.Synchronised) return;

            Configuration.Synchronised = false;

            Send("2", $"7={Configuration.InboundSeqNum + 1}|16=0|");
        }

        public void Send(string type, string bodypart)
        {
            var message = Extensions.Prepare(Configuration, type, bodypart);
            var data = Encoding.UTF8.GetBytes(message);

            Transport.Outbound.Write(data, 0, data.Length);

            Configuration.OutboundSeqNum++;
            Configuration.OutboundUpdatedAt = DateTime.UtcNow;
        }

        public void Dispose()
        {
            Transport?.Dispose();
        }
    }

    public static class Extensions
    {
        public static string Prepare(IConfiguration configuration, string type, string bodypart)
        {
            // NOTE: This is temporary until we implement an efficient message builder

            var version = configuration.Version;
            var sender = configuration.Sender;
            var target = configuration.Target;
            var seqnum = configuration.OutboundSeqNum;
            var timestamp = DateTime.UtcNow;

            var body = $"35={type}|34={seqnum + 1}|49={sender}|52={timestamp:yyyyMMdd-HH:mm:ss.fff}|56={target}|{bodypart}".Replace("|", "\u0001");
            var header = $"8={version}|9={body.Length}|".Replace("|", "\u0001");
            var trailer = $"10={((header + body).ToCharArray().Select(x => (int)x).Sum() % 256):D3}|".Replace("|", "\u0001");

            var message = header + body + trailer;

            return message;
        }
    }
}
