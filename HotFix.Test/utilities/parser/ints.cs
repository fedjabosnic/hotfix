using System;
using System.Text;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.parser
{
    [TestClass]
    public class ints
    {
        [TestMethod]
        public void zero()
        {
            Encoding.ASCII.GetBytes("0").GetInt().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            Encoding.ASCII.GetBytes("123").GetInt().Should().Be(123);
        }

        [TestMethod]
        public void positive_with_leading_zeros()
        {
            Encoding.ASCII.GetBytes("000123").GetInt().Should().Be(123);
        }

        [TestMethod]
        public void negative()
        {
            Encoding.ASCII.GetBytes("-123").GetInt().Should().Be(-123);
        }

        [TestMethod]
        public void negative_with_leading_zeros()
        {
            Encoding.ASCII.GetBytes("-000123").GetInt().Should().Be(-123);
        }
    }
}