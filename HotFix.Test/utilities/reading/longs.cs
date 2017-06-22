using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.reading
{
    [TestClass]
    public class longs
    {
        [TestMethod]
        public void zero()
        {
            System.Text.Encoding.ASCII.GetBytes("0").ReadLong().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            System.Text.Encoding.ASCII.GetBytes("1234567890987654321").ReadLong().Should().Be(1234567890987654321L);
        }

        [TestMethod]
        public void positive_with_leading_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("0001234567890987654321").ReadLong().Should().Be(1234567890987654321L);
        }

        [TestMethod]
        public void negative()
        {
            System.Text.Encoding.ASCII.GetBytes("-1234567890987654321").ReadLong().Should().Be(-1234567890987654321L);
        }

        [TestMethod]
        public void negative_with_leading_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("-0001234567890987654321").ReadLong().Should().Be(-1234567890987654321L);
        }
    }
}