using System;
using System.Text;
using FluentAssertions;
using HotFix.Encoding;
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

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "0.0" + "\0\0\0\0\0\0\0");
            written.Should().Be(3);
        }

        [TestMethod]
        public void positive()
        {
            var written = _buffer.WriteFloat(1, 78.123456);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "78.123456" + "\0");
            written.Should().Be(9);
        }

        [TestMethod]
        public void negative()
        {
            var written = _buffer.WriteFloat(1, -78.123456);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "-78.123456");
            written.Should().Be(10);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void out_of_bounds()
        {
            _buffer.WriteFloat(7, 78.123456);
        }
    }
}
