using System;
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
        public void zero_no_decimals()
        {
            var written = _buffer.WriteFloat(1, 0, 0);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "0" + "\0\0\0\0\0\0\0\0\0");
            written.Should().Be(1);
        }

        [TestMethod]
        public void zero_with_decimals()
        {
            var written = _buffer.WriteFloat(1, 0, 6);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "0.000000" + "\0\0");
            written.Should().Be(8);
        }

        [TestMethod]
        public void positive_no_decimals()
        {
            var written = _buffer.WriteFloat(1, 78.003456, 0);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "78" + "\0\0\0\0\0\0\0\0");
            written.Should().Be(2);
        }

        [TestMethod]
        public void positive_with_decimals()
        {
            var written = _buffer.WriteFloat(1, 78.003456, 6);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "78.003456" + "\0");
            written.Should().Be(9);
        }

        [TestMethod]
        public void positive_with_decimals_rounded_down()
        {
            var written = _buffer.WriteFloat(1, 78.003456, 3);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "78.003" + "\0\0\0\0");
            written.Should().Be(6);
        }

        [TestMethod]
        public void positive_with_decimals_rounded_up()
        {
            var written = _buffer.WriteFloat(1, 78.003456, 4);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "78.0035" + "\0\0\0");
            written.Should().Be(7);
        }

        [TestMethod]
        public void negative_no_decimals()
        {
            var written = _buffer.WriteFloat(1, -78.003456, 0);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "-78" + "\0\0\0\0\0\0\0");
            written.Should().Be(3);
        }

        [TestMethod]
        public void negative_with_decimals()
        {
            var written = _buffer.WriteFloat(1, -78.003456, 6);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "-78.003456" + "");
            written.Should().Be(10);
        }

        [TestMethod]
        public void negative_with_decimals_rounded_down()
        {
            var written = _buffer.WriteFloat(1, -78.003456, 3);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "-78.003" + "\0\0\0");
            written.Should().Be(7);
        }

        [TestMethod]
        public void negative_with_decimals_rounded_up()
        {
            var written = _buffer.WriteFloat(1, -78.003456, 4);

            System.Text.Encoding.ASCII.GetString(_buffer).Should().Be("\0" + "-78.0035" + "\0\0");
            written.Should().Be(8);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void out_of_bounds()
        {
            _buffer.WriteFloat(7, 78.123456, 6);
        }
    }
}
