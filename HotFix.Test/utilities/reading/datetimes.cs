using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.reading
{
    [TestClass]
    public class datetimes
    {
        [TestMethod]
        public void timestamp()
        {
            var result = "20170327-15:45:13".AsBytes().ReadDateTime();

            result.Should().Be("2017/03/27 15:45:13.000".AsDateTime());
        }

        [TestMethod]
        public void timestamp_with_milliseconds()
        {
            var result = "20170327-15:45:13.596".AsBytes().ReadDateTime();

            result.Should().Be("2017/03/27 15:45:13.596".AsDateTime());
        }
    }
}
