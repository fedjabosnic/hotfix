using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.Core
{
    [TestClass]
    public class Messages
    {
        [TestMethod]
        public void parsing_fields_works()
        {
            var raw = (
                "8=FIX.4.2|9=968|35=X|34=53677|52=20170525-00:55:16.153|49=SENDER..|56=RECEIVER..........|262=c6424b19-af74-4c17-8266-9c52ca583ad2" +
                "|268=8" +
                "|279=2|55=GBP/JPY|269=0|278=1211918436|270=144.808000|271=1000000.000000|110=0.000000|15=GBP|282=290" +
                "|279=2|55=GBP/JPY|269=0|278=1211918437|270=144.802000|271=2000000.000000|110=0.000000|15=GBP|282=290" +
                "|279=0|55=GBP/JPY|269=0|278=1211918501|270=144.809000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                "|279=0|55=GBP/JPY|269=0|278=1211918502|270=144.803000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                "|279=2|55=GBP/JPY|269=1|278=1211918438|270=144.826000|271=1000000.000000|110=0.000000|15=GBP|282=290" +
                "|279=2|55=GBP/JPY|269=1|278=1211918439|270=144.833000|271=2000000.000000|110=0.000000|15=GBP|282=290" +
                "|279=0|55=GBP/JPY|269=1|278=1211918503|270=144.828000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                "|279=0|55=GBP/JPY|269=1|278=1211918504|270=144.834000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                "|10=161|").Replace("|", "\u0001");

            var message = new Message().Parse(raw);

            // Get sending time as a datetime
            message[52].DateTime.Should().Be(DateTime.Parse("25/05/2017 00:55:16.153"));

            // Get prices from the groups
            message[270, 0].Float.Should().Be(144.808d);
            message[270, 1].Float.Should().Be(144.802d);
            message[270, 2].Float.Should().Be(144.809d);
            message[270, 3].Float.Should().Be(144.803d);
            message[270, 4].Float.Should().Be(144.826d);
            message[270, 5].Float.Should().Be(144.833d);
            message[270, 6].Float.Should().Be(144.828d);
            message[270, 7].Float.Should().Be(144.834d);
        }
    }
}