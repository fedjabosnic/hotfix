using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.Core.message
{
    [TestClass]
    public class retrieve_a_field
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
        public void succeeds_when_the_field_exists()
        {
            Message.Parse(Logon);

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
        public void fails_when_the_field_does_not_exist()
        {
            var _ = Message[999];
        }
    }
}
