using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.message
{
    [TestClass]
    public class writing_to_a_byte_array
    {
        [TestMethod]
        public void writes_the_whole_message_to_the_given_buffer_starting_at_the_correct_offset()
        {
            var message = new FIXMessage();
            var logon = "8=FIX.4.2|9=70|35=A|34=1|52=20170607-12:17:52.355|49=DAEV|56=TARGET|98=0|108=5|141=Y|10=231|".Replace("|", "\u0001");
            var bytes = logon.AsBytes();
            var target = new byte[100];

            message
                .Parse(bytes, 0, bytes.Length)
                .WriteTo(target, 3);

            var result = System.Text.Encoding.ASCII.GetString(target);

            result.Should().Be("\0\0\0" + logon + "\0\0\0\0\0");
        }
    }
}
