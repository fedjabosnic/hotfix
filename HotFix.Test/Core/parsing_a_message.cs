using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.Core
{
    [TestClass]
    public class parsing_a_message
    {
        public Message Message { get; set; }
        public string Logon { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Message = new Message();
            Logon = "8=FIX.4.2|9=70|35=A|34=1|52=20170607-12:17:52.355|49=DAEV|56=TARGET|98=0|108=5|141=Y|10=231|".Replace("|", "\u0001");
        }

        [TestMethod]
        public void succeeds_when_the_message_is_valid()
        {
            Message.Parse(Logon);

            Message.Count.Should().Be(11);

            Message[  8].String.Should().Be("FIX.4.2");
            Message[  9].String.Should().Be("70");
            Message[ 35].String.Should().Be("A");
            Message[ 34].String.Should().Be("1");
            Message[ 52].String.Should().Be("20170607-12:17:52.355");
            Message[ 49].String.Should().Be("DAEV");
            Message[ 56].String.Should().Be("TARGET");
            Message[ 98].String.Should().Be("0");
            Message[108].String.Should().Be("5");
            Message[141].String.Should().Be("Y");
            Message[ 10].String.Should().Be("231");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_the_beginstring_field_is_missing()
        {
            Logon = Logon.Replace("8=FIX.4.2\u0001", "");

            Message.Parse(Logon);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_the_bodylength_field_is_missing()
        {
            Logon = Logon.Replace("9=70\u0001", "");

            Message.Parse(Logon);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_the_msgtype_field_is_missing()
        {
            Logon = Logon.Replace("35=A\u0001", "");

            Message.Parse(Logon);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_the_checksum_field_is_missing()
        {
            Logon = Logon.Replace("10=231\u0001", "");

            Message.Parse(Logon);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_a_tag_is_missing()
        {
            Logon = Logon.Replace("34=1\u0001", "=1\u0001");

            Message.Parse(Logon);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_a_tag_is_not_a_number()
        {
            Logon = Logon.Replace("34=1\u0001", "3X=1\u0001");

            Message.Parse(Logon);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void fails_when_a_value_is_missing()
        {
            Logon = Logon.Replace("34=1\u0001", "34=\u0001");

            Message.Parse(Logon);
        }
    }
}