using System;
using System.Threading;
using HotFix.Core;

namespace HotFix.Demo.Acceptor
{
    /// <summary>
    /// This demo is an acceptor implemented with the event driven api. It will wait for and accept a single inbound connection;
    /// once a session has been established it will immediately execute any orders that it receives.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();

            var host = args[0];
            var port = int.Parse(args[1]);
            var orders = 0;
            var executions = 0;

            var engine = new Engine();

            var configuration = new Configuration
            {
                Role = Role.Acceptor,
                Version = "FIX.4.2",
                Host = host,
                Port = port,
                Sender = "Server",
                Target = "Client",
                InboundSeqNum = 1,
                OutboundSeqNum = 1,
                HeartbeatInterval = 0,
                //LogFile = @"messages.log" // Enable to see logging impact
            };

            while (true)
            {
                Console.WriteLine("Waiting for client...");
                Console.WriteLine();

                engine.Run(
                    configuration,
                    session =>
                    {
                        Console.WriteLine("Logged on");
                    },
                    session =>
                    {
                        Console.WriteLine("Logged out");
                    },
                    (session, message) =>
                    {
                        if (message[35].Is("D"))
                        {
                            // Immediately fill any order at the requested price
                            var report = session
                                .Outbound
                                .Clear()
                                .Set(37, ++orders) // OrderId
                                .Set(17, ++executions) // ExecId
                                .Set(20, 0) // ExecTransType (New)
                                .Set(150, 2) // ExecType (Fill)
                                .Set(39, 2) // OrdStatus (Filled)
                                .Set(11, message[11]) // ClOrdId
                                .Set(55, message[55]) // Symbol
                                .Set(54, message[54]) // Side
                                .Set(38, message[38]) // OrderQty
                                .Set(44, message[44]) // Price
                                .Set(06, message[44]) // AvgPrice
                                .Set(14, message[38]) // CumQty
                                .Set(151, 0); // LeavesQty

                            session.Send("8", report);

                            return true;
                        }

                        return false;
                    });

                Console.WriteLine();
                Console.WriteLine("Orders: " + orders);
                Console.WriteLine("Filled: " + executions);
                Console.WriteLine();

                Thread.Sleep(1000);
            }
        }
    }
}
