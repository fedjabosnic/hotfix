using System;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.parser
{
    [TestClass]
    public class datetimes
    {
        [TestMethod]
        public void timestamp()
        {
            var result = "20170327-15:45:13".GetDateTime();

            result.Should().Be(DateTime.Parse("27/03/2017 15:45:13"));
        }

        [TestMethod]
        public void timestamp_with_milliseconds()
        {
            var result = "20170327-15:45:13.596".GetDateTime();

            result.Should().Be(DateTime.Parse("27/03/2017 15:45:13.596"));
        }
    }
}
