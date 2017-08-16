using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static int GC0;
        public static int GC1;
        public static int GC2;

        private static int Orders;
        private static int Executions;

        static void Main(string[] args)
        {
            Console.WriteLine();

            var host = args[0];
            var port = int.Parse(args[1]);

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
                Schedules = new List<ISchedule>
                {
                    new Schedule
                    {
                        Name = "Session",
                        OpenDay = DayOfWeek.Monday,
                        OpenTime = TimeSpan.Parse("00:00:00"),
                        CloseDay = DayOfWeek.Sunday,
                        CloseTime = TimeSpan.Parse("23:59:59")
                    }
                }
            };

            engine.Run(
                configuration,
                Logon,
                Logout,
                (session, message) =>
                {
                    if (message[35].Is("D"))
                    {
                        // Immediately fill any order at the requested price
                        var report = session
                            .Outbound
                            .Clear()
                            .Set(37, ++Orders)     // OrderId
                            .Set(17, ++Executions) // ExecId
                            .Set(20, 0)            // ExecTransType (New)
                            .Set(150, 2)           // ExecType (Fill)
                            .Set(39, 2)            // OrdStatus (Filled)
                            .Set(11, message[11])  // ClOrdId
                            .Set(55, message[55])  // Symbol
                            .Set(54, message[54])  // Side
                            .Set(38, message[38])  // OrderQty
                            .Set(44, message[44])  // Price
                            .Set(06, message[44])  // AvgPrice
                            .Set(14, message[38])  // CumQty
                            .Set(151, 0);          // LeavesQty

                        session.Send("8", report);

                        return true;
                    }

                    return false;
                });
        }

        private static void Logon(Session session)
        {
            Console.WriteLine("Logged on");

            Orders = 0;
            Executions = 0;

            GC.Collect(0, GCCollectionMode.Forced, true, true);
            GC.Collect(1, GCCollectionMode.Forced, true, true);
            GC.Collect(2, GCCollectionMode.Forced, true, true);

            GC0 = GC.CollectionCount(0);
            GC1 = GC.CollectionCount(1);
            GC2 = GC.CollectionCount(2);

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Thread.BeginThreadAffinity();
        }

        private static void Logout(Session session)
        {
            Console.WriteLine("Logged out");

            Thread.EndThreadAffinity();

            GC0 = GC.CollectionCount(0) - GC0;
            GC1 = GC.CollectionCount(1) - GC1;
            GC2 = GC.CollectionCount(2) - GC2;

            Console.WriteLine();
            Console.WriteLine("Orders: " + Orders);
            Console.WriteLine("Filled: " + Executions);
            Console.WriteLine();
            Console.WriteLine($"GC 0: {GC0}");
            Console.WriteLine($"GC 1: {GC1}");
            Console.WriteLine($"GC 2: {GC2}");
            Console.WriteLine();

            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.WriteLine("Waiting for client...");
            Console.WriteLine();
        }
    }
}
