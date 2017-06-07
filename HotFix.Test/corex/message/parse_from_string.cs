using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.Core.message
{
    [TestClass]
    public class parse_from_string
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