using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
            Configuration.LoggingOn = true;

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
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"! {e.Message}");
                }
                finally
                {
                    if (tail == head) tail = head = 0;
                }

                CheckSessionState();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Process(Message message)
        {
            if (!message.Valid) return;

            Configuration.InboundUpdatedAt = DateTime.UtcNow;

            if (!VerifyBeginString(message)) return; // Logout and disconnect
            if (!VerifyParties(message)) return; // Session level Reject, increment inbound SeqNum, logout and then disconnect
            if (!VerifySeqNum(message)) return; // Too high: resend request, too low: error and exit
            if (!VerifyLoggedOn(message)) return; // Error and disconnect

            switch (message[35].AsString)
            {
                case "0":
                    VerifyTestResponse(message);
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Heartbeat");
                    break;
                case "A":
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Logon");
                    break;
                case "5":
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Logout");
                    break;
                case "3":
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Reject");
                    break;
                case "2":
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Resend request");
                    break;
                case "4":
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Sequence reset");
                    break;
                case "1":
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Test request");
                    break;
                default:
                    Debug.WriteLine($"< {message[52].AsDateTime:yyyyMMdd HH:mm:ss.fff}: Application message ({message[35].AsString})");
                    break;
            }

            Inbound?.Invoke(message);

            Configuration.InboundSeqNum++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckSessionState()
        {
            if (OutboundHeartbeatExpired()) Send("0", "");
            if (InboundHeartbeatExpired()) Send("1", $"112={Configuration.OutboundTestReqId = DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss.fff")}|");
            if (InboundTestResponseExpired()) throw new Exception("Didn't receive a test response");
        }

        /// <summary>
        /// Returns true when no messages have been sent for longer than the heartbeat interval.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool OutboundHeartbeatExpired()
        {
            var now = DateTime.UtcNow;
            var last = Configuration.OutboundUpdatedAt;
            var timeout = TimeSpan.FromSeconds(Configuration.HeartbeatInterval);

            return now - last > timeout;
        }

        /// <summary>
        /// Returns true when no messages have been received for longer than the heartbeat interval.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InboundHeartbeatExpired()
        {
            if (Configuration.OutboundTestReqId != null) return false;

            var now = DateTime.UtcNow;
            var last = Configuration.InboundUpdatedAt;
            var timeout = TimeSpan.FromSeconds(Configuration.HeartbeatInterval * 1.2);

            return now - last > timeout;
        }

        /// <summary>
        /// Returns true when a requested test response has not been received for longer than the heartbeat interval.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InboundTestResponseExpired()
        {
            if (Configuration.OutboundTestReqId == null) return false;

            var now = DateTime.UtcNow;
            var last = Configuration.OutboundTestReqId.GetDateTime();
            var timeout = TimeSpan.FromSeconds(Configuration.HeartbeatInterval * 2);

            return now - last > timeout;
        }

        /// <summary>
        /// Returns true if the message specifies the expected begin string (fix protocol version).
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool VerifyBeginString(Message message)
        {
            // TODO: Error/fail here?
            return message[8].AsString == Configuration.Version;
        }

        /// <summary>
        /// Returns true if the message specifies the expected target and sender identifiers.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool VerifyParties(Message message)
        {
            // TODO: Error/fail here?
            return message[49].AsString == Configuration.Target && message[56].AsString == Configuration.Sender;
        }

        /// <summary>
        /// Returns true if the message specifies the expected sequence number. If the sequence number is higher
        /// than expected, a resend request will be sent to fill the gap (unless one is already in flight) and
        /// false will be returned. If the sequence number is lower than expected, an exception will be thrown.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool VerifySeqNum(Message message)
        {
            var seqnum = message[34].AsLong;
            var expected = Configuration.InboundSeqNum + 1;

            if (seqnum > expected)
            {
                if (Configuration.Synchronised)
                {
                    Configuration.Synchronised = false;

                    // Send resend request
                    Send("2", $"7={Configuration.InboundSeqNum + 1}|16=0|");
                }

                return false;
            }

            if (seqnum < expected)
            {
                throw new Exception("Inbound sequence number too low"); // Disconnect and error
            }

            Configuration.Synchronised = true;

            return true;
        }

        /// <summary>
        /// Returns true if the session is already logged on or this message is a logon response. Any message
        /// other than logon received while the session is not logged on will cause termination of the session.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool VerifyLoggedOn(Message message)
        {
            if (Configuration.LoggedOn) return true;

            if (Configuration.LoggingOn && message[35].AsString == "A")
            {
                // TODO: Validate logon message
                Configuration.LoggedOn = true;
                Configuration.LoggingOn = false;
                return true;
            }

            throw new Exception("First message not a logon");
        }

        /// <summary>
        /// Fulfills the current test request if the message contains a valid test response.
        /// </summary>
        /// <param name="message"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void VerifyTestResponse(Message message)
        {
            // Not awaiting a test response
            if (Configuration.OutboundTestReqId == null) return;

            // If there is a test id, confirm that it's valid
            if (message.Contains(112) && message[112].AsString == Configuration.OutboundTestReqId)
            {
                Configuration.OutboundTestReqId = null;
            }
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
