using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.reading
{
    [TestClass]
    public class ints
    {
        [TestMethod]
        public void zero()
        {
            System.Text.Encoding.ASCII.GetBytes("0").ReadInt().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            System.Text.Encoding.ASCII.GetBytes("123").ReadInt().Should().Be(123);
        }

        [TestMethod]
        public void positive_with_leading_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("000123").ReadInt().Should().Be(123);
        }

        [TestMethod]
        public void negative()
        {
            System.Text.Encoding.ASCII.GetBytes("-123").ReadInt().Should().Be(-123);
        }

        [TestMethod]
        public void negative_with_leading_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("-000123").ReadInt().Should().Be(-123);
        }
    }
}