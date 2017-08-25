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
            "0".AsBytes().ReadFloat().Should().Be(0d);
        }

        [TestMethod]
        public void zero_with_decimals()
        {
            "0.0".AsBytes().ReadFloat().Should().Be(0.0d);
        }

        [TestMethod]
        public void positive()
        {
            "123".AsBytes().ReadFloat().Should().Be(123d);
        }

        [TestMethod]
        public void positive_with_decimals()
        {
            "123.456789".AsBytes().ReadFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void positive_with_decimals_and_leading_zeros()
        {
            "000123.456789".AsBytes().ReadFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void positive_with_decimals_and_trailing_zeros()
        {
            "123.456789000".AsBytes().ReadFloat().Should().Be(123.456789d);
        }

        [TestMethod]
        public void negative()
        {
            "-123".AsBytes().ReadFloat().Should().Be(-123d);
        }

        [TestMethod]
        public void negative_with_decimals()
        {
            "-123.456789".AsBytes().ReadFloat().Should().Be(-123.456789d);
        }

        [TestMethod]
        public void negative_with_decimals_and_leading_zeros()
        {
            "-000123.456789".AsBytes().ReadFloat().Should().Be(-123.456789d);
        }

        [TestMethod]
        public void negative_with_decimals_and_trailing_zeros()
        {
            "-123.456789000".AsBytes().ReadFloat().Should().Be(-123.456789d);
        }
    }
}
