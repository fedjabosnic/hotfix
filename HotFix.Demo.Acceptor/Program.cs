using System;
using HotFix.Core;

namespace HotFix.Demo.Acceptor
{
    class Program
    {
        private static int _orders;
        private static int _executions;

        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for orders...");

            var engine = new Engine();

            var configuration = new Configuration
            {
                Role = Role.Acceptor,
                Version = "FIX.4.2",
                Host = "127.0.0.1",
                Port = int.Parse(args[0]),
                Sender = "Server",
                Target = "Client",
                InboundSeqNum = 1,
                OutboundSeqNum = 1,
                HeartbeatInterval = 86400
            };

            using (var session = engine.Open(configuration))
            {
                var clock = session.Clock;
                var state = session.State;
                var sender = session.Configuration.Sender;
                var target = session.Configuration.Target;

                var inbound = session.Inbound;
                var outbound = session.Outbound;

                session.Logon();

                while (session.Active)
                {
                    session.Receive();

                    if (!inbound.Valid || !inbound[35].Is("D")) continue;

                    outbound.Prepare("8");

                    outbound.Set(34, state.OutboundSeqNum);
                    outbound.Set(52, clock.Time);
                    outbound.Set(49, sender);
                    outbound.Set(56, target);

                    outbound.Set(37, ++_orders);     // OrderId 
                    outbound.Set(11, ++_executions); // ExecId 
                    outbound.Set(20, 0);             // ExecTransType (New) 
                    outbound.Set(150, 2);            // ExecType (Fill) 
                    outbound.Set(39, 2);             // OrdStatus (Filled) 

                    outbound.Set(11, inbound[11]);   // ClOrdId 
                    outbound.Set(55, inbound[55]);   // Symbol 
                    outbound.Set(54, inbound[54]);   // Side 
                    outbound.Set(38, inbound[38]);   // OrderQty 
                    outbound.Set(44, inbound[44]);   // Price 
                    outbound.Set(6,  inbound[44]);   // AvgPrice 
                    outbound.Set(14, inbound[38]);   // CumQty 
                    outbound.Set(151, 0);            // LeavesQty 

                    outbound.Build();

                    session.Send(state, session.Channel, outbound);
                }

                session.Logout();
            }

            Console.WriteLine();
            Console.WriteLine("Orders: " + _orders);
            Console.WriteLine("Filled: " + _executions);
        }
    }
}
