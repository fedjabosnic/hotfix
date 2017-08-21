using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.reading
{
    [TestClass]
    public class strings
    {
        [TestMethod]
        public void empty()
        {
            "".AsBytes().ReadString().Should().Be("");
        }

        [TestMethod]
        public void small()
        {
            "XXX".AsBytes().ReadString().Should().Be("XXX");
        }

        [TestMethod]
        public void large()
        {
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;'#][/=-_+{}~@:?><".AsBytes().ReadString().Should().Be("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;'#][/=-_+{}~@:?><");
        }
    }
}