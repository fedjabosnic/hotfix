using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.message
{
    [TestClass]
    public class checking_for_a_field
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
        public void returns_true_when_the_field_exists()
        {
            Message.Parse(Logon);

            Message.Contains(8).Should().Be(true);
            Message.Contains(9).Should().Be(true);
            Message.Contains(35).Should().Be(true);
            Message.Contains(34).Should().Be(true);
            Message.Contains(52).Should().Be(true);
            Message.Contains(49).Should().Be(true);
            Message.Contains(56).Should().Be(true);
            Message.Contains(98).Should().Be(true);
            Message.Contains(108).Should().Be(true);
            Message.Contains(141).Should().Be(true);
            Message.Contains(10).Should().Be(true);
        }

        [TestMethod]
        public void returns_false_when_the_field_does_not_exist()
        {
            Message.Contains(999).Should().Be(false);
        }
    }
}