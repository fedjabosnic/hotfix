using System;
using System.Text;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.writing
{
    [TestClass]
    public class floats
    {
        private byte[] _buffer;

        [TestInitialize]
        public void Setup()
        {
            _buffer = new byte[11];
        }

        [TestMethod]
        public void zero()
        {
            var written = _buffer.WriteFloat(1, 0);

            Encoding.ASCII.GetString(_buffer).Should().Be("\00.0\0\0\0\0\0\0\0");
            written.Should().Be(3);
        }

        [TestMethod]
        public void positive()
        {
            var written = _buffer.WriteFloat(1, 78.123456);

            Encoding.ASCII.GetString(_buffer).Should().Be("\078.123456\0");
            written.Should().Be(9);
        }

        [TestMethod]
        public void negative()
        {
            var written = _buffer.WriteFloat(1, -78.123456);

            Encoding.ASCII.GetString(_buffer).Should().Be("\0-78.123456");
            written.Should().Be(10);
        }
    }
}
