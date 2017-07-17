using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.messagewriter
{
    [TestClass]
    public class writing_to_a_byte_array
    {
        [TestMethod]
        public void writes_the_whole_message_to_the_given_buffer_starting_at_the_correct_offset()
        {
            var message = new FIXMessageWriter(1000);
            var logon = "8=FIX.4.2|9=00069|35=A|34=177|52=20090107-18:15:16.000|49=SERVER|56=CLIENT|98=0|108=30|10=144|".Replace("|", "\u0001");
            var target = new byte[100];

            message
                .Set(98, 0)
                .Set(108, 30)
                .Prepare("FIX.4.2", "A", 177, DateTime.ParseExact("2009/01/07 18:15:16", "yyyy/MM/dd HH:mm:ss", null), "SERVER", "CLIENT")
                .WriteTo(target, 3);

            var result = System.Text.Encoding.ASCII.GetString(target);
            result.Should().Be("\0\0\0" + logon + "\0\0\0");
        }
    }
}
