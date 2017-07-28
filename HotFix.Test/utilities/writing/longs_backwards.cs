using System;
using FluentAssertions;
using HotFix.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.writing
{
    [TestClass]
    public class longs_backwards
    {
        private byte[] _buffer;

        [TestInitialize]
        public void Setup()
        {
            _buffer = new byte[24];
        }

        [TestMethod]
        public void zero()
        {
            var written = _buffer.WriteLongBackwards(20, 0);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" + "0" + "\0\0\0");
            written.Should().Be(1);
        }

        [TestMethod]
        public void positive()
        {
            var written = _buffer.WriteLongBackwards(20, 1234567890987654321);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0\0" + "1234567890987654321" + "\0\0\0");
            written.Should().Be(19);
        }

        [TestMethod]
        public void negative()
        {
            var written = _buffer.WriteLongBackwards(20, -1234567890987654321);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "-1234567890987654321" + "\0\0\0");
            written.Should().Be(20);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void out_of_bounds()
        {
            _buffer.WriteLongBackwards(7, 1234567890987654321);
        }
    }
}