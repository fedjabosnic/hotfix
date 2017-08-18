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
            "0".AsBytes().ReadInt().Should().Be(0);
        }

        [TestMethod]
        public void positive()
        {
            "123".AsBytes().ReadInt().Should().Be(123);
        }

        [TestMethod]
        public void positive_with_leading_zeros()
        {
            "000123".AsBytes().ReadInt().Should().Be(123);
        }

        [TestMethod]
        public void negative()
        {
            "-123".AsBytes().ReadInt().Should().Be(-123);
        }

        [TestMethod]
        public void negative_with_leading_zeros()
        {
            "-000123".AsBytes().ReadInt().Should().Be(-123);
        }
    }
}