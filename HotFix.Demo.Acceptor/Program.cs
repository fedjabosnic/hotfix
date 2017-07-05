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
                var state = session.State;

                var inbound = session.Inbound;
                var outbound = session.Outbound;

                session.Logon();

                while (session.Active)
                {
                    session.Receive();

                    if (!inbound.Valid || !inbound[35].Is("D")) continue;

                    outbound
                        .Clear()
                        .Set(37, ++_orders)     // OrderId 
                        .Set(11, ++_executions) // ExecId 
                        .Set(20, 0)             // ExecTransType (New) 
                        .Set(150, 2)            // ExecType (Fill) 
                        .Set(39, 2)             // OrdStatus (Filled) 
                        .Set(11, inbound[11])   // ClOrdId 
                        .Set(55, inbound[55])   // Symbol 
                        .Set(54, inbound[54])   // Side 
                        .Set(38, inbound[38])   // OrderQty 
                        .Set(44, inbound[44])   // Price 
                        .Set(6,  inbound[44])   // AvgPrice 
                        .Set(14, inbound[38])   // CumQty 
                        .Set(151, 0);            // LeavesQty 

                    session.Send("8", state, session.Channel, outbound);
                }

                session.Logout();
            }

            Console.WriteLine();
            Console.WriteLine("Orders: " + _orders);
            Console.WriteLine("Filled: " + _executions);
        }
    }
}
