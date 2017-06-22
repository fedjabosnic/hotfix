using System;
using System.Text;
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
            System.Text.Encoding.ASCII.GetBytes("").GetString().Should().Be("");
        }

        [TestMethod]
        public void small()
        {
            System.Text.Encoding.ASCII.GetBytes("XXX").GetString().Should().Be("XXX");
        }

        [TestMethod]
        public void large()
        {
            System.Text.Encoding.ASCII.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;'#][/=-_+{}~@:?><").GetString().Should().Be("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;'#][/=-_+{}~@:?><");
        }
    }
}