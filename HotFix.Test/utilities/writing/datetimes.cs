using System;
using System.Text;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.writing
{
    [TestClass]
    public class datetimes
    {
        private byte[] _buffer;

        [TestInitialize]
        public void Setup()
        {
            _buffer = new byte[23];
        }

        [TestMethod]
        public void timestamp()
        {
            var written = _buffer.WriteDateTime(1, DateTime.Parse("27/03/2017 15:45:13"));

            Encoding.ASCII.GetString(_buffer).Should().Be("\020170327-15:45:13\0\0\0\0\0");
            written.Should().Be(17);
        }

        [TestMethod]
        public void timestamp_with_milliseconds()
        {
            var written = _buffer.WriteDateTime(1, DateTime.Parse("27/03/2017 15:45:13.123"));

            Encoding.ASCII.GetString(_buffer).Should().Be("\020170327-15:45:13.123\0");
            written.Should().Be(21);
        }
    }
}
