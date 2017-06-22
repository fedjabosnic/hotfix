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
            System.Text.Encoding.ASCII.GetBytes("").ReadString().Should().Be("");
        }

        [TestMethod]
        public void small()
        {
            System.Text.Encoding.ASCII.GetBytes("XXX").ReadString().Should().Be("XXX");
        }

        [TestMethod]
        public void large()
        {
            System.Text.Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;'#][/=-_+{}~@:?><").ReadString().Should().Be("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;'#][/=-_+{}~@:?><");
        }
    }
}