using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.field
{
    [TestClass]
    public class checking_the_value_for_string_equality
    {
        [TestMethod]
        public void returns_false_when_the_value_is_not_equal_to_the_input()
        {
            var field = new Field(System.Text.Encoding.ASCII.GetBytes("34=XXXXXX\u0001"), 0, new Segment(3, 6), 10, 0);

            field.Is("ABCDEF").Should().Be(false);
        }

        [TestMethod]
        public void returns_true_when_the_value_is_equal_to_the_input()
        {
            var field = new Field(System.Text.Encoding.ASCII.GetBytes("34=ABCDEF\u0001"), 0, new Segment(3, 6), 10, 0);

            field.Is("ABCDEF").Should().Be(true);
        }
    }
}