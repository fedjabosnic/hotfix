using System;
using System.Text;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.writing
{
    [TestClass]
    public class ints
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
            var written = _buffer.WriteInt(3, 0);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0\0\0" + "0" + "\0\0\0\0\0\0\0");
            written.Should().Be(1);
        }

        [TestMethod]
        public void positive()
        {
            var written = _buffer.WriteInt(3, 12345);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0\0\0" + "12345" + "\0\0\0");
            written.Should().Be(5);
        }

        [TestMethod]
        public void negative()
        {
            var written = _buffer.WriteInt(3, -12345);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0\0\0" + "-12345" + "\0\0");
            written.Should().Be(6);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void out_of_bounds()
        {
            _buffer.WriteInt(7, 12345);
        }
    }
}