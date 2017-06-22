using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.reading
{
    [TestClass]
    public class floats
    {
        [TestMethod]
        public void zero()
        {
            System.Text.Encoding.ASCII.GetBytes("0").ReadFloat().Should().Be(0d);
        }

        [TestMethod]
        public void zero_with_decimals()
        {
            System.Text.Encoding.ASCII.GetBytes("0.0").ReadFloat().Should().Be(0.0d);
        }

        [TestMethod]
        public void positive()
        {
            System.Text.Encoding.ASCII.GetBytes("123").ReadFloat().Should().Be(123d);
        }

        [TestMethod]
        public void positive_with_decimals()
        {
            System.Text.Encoding.ASCII.GetBytes("123.456789").ReadFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void positive_with_decimals_and_leading_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("000123.456789").ReadFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void positive_with_decimals_and_trailing_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("123.456789000").ReadFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void negative()
        {
            System.Text.Encoding.ASCII.GetBytes("-123").ReadFloat().Should().Be(-123d);
        }

        [TestMethod]
        public void negative_with_decimals()
        {
            System.Text.Encoding.ASCII.GetBytes("-123.456789").ReadFloat().Should().Be(-123.456789d);
        }

        [TestMethod]
        public void negative_with_decimals_and_leading_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("-000123.456789").ReadFloat().Should().Be(-123.456789d);
        }

        [TestMethod]
        public void negative_with_decimals_and_trailing_zeros()
        {
            System.Text.Encoding.ASCII.GetBytes("-123.456789000").ReadFloat().Should().Be(-123.456789d);
        }
    }
}
