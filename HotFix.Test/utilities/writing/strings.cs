using System;
using System.Text;
using FluentAssertions;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.writing
{
    [TestClass]
    public class strings
    {
        private byte[] _buffer;

        [TestInitialize]
        public void Setup()
        {
            _buffer = new byte[11];
        }

        [TestMethod]
        public void empty()
        {
            var written = _buffer.WriteString(3, "");

            Encoding.ASCII.GetString(_buffer).Should().Be("\0\0\0\0\0\0\0\0\0\0\0");
            written.Should().Be(0);
        }

        [TestMethod]
        public void non_empty()
        {
            var written = _buffer.WriteString(3, "string");

            Encoding.ASCII.GetString(_buffer).Should().Be("\0\0\0" + "string" + "\0\0");
            written.Should().Be(6);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void out_of_bounds()
        {
            _buffer.WriteString(7, "string");
        }
    }
}
