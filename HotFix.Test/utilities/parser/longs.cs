using System;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.parser
{
    [TestClass]
    public class longs
    {
        [TestMethod]
        public void zero()
        {
            "0".GetLong().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            "1234567890987654321".GetLong().Should().Be(1234567890987654321L);
        }

        [TestMethod]
        public void positive_with_leading_zeros()
        {
            "0001234567890987654321".GetLong().Should().Be(1234567890987654321L);
        }

        [TestMethod]
        public void negative()
        {
            "-1234567890987654321".GetLong().Should().Be(-1234567890987654321L);
        }

        [TestMethod]
        public void negative_with_leading_zeros()
        {
            "-0001234567890987654321".GetLong().Should().Be(-1234567890987654321L);
        }
    }
}