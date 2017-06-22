using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.message
{
    [TestClass]
    public class converting_to_string
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
        public void returns_the_original_string()
        {
            Message.Parse(System.Text.Encoding.ASCII.GetBytes(Logon), 0, Logon.Length);

            Message.ToString().Should().Be(Logon);
        }
    }
}