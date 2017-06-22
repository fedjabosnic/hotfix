using System;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.core.field
{
    [TestClass]
    public class checking_the_value_for_datetime_equality
    {
        [TestMethod]
        public void returns_false_when_the_value_is_not_a_valid_datetime()
        {
            var field = new Field(System.Text.Encoding.ASCII.GetBytes("34=XXXXXX-YY:YY:YY.ZZZ\u0001"), 0, new Segment(3, 21), 25, 0);

            field.Is(DateTime.Parse("27/03/2017 09:34:21.456")).Should().Be(false);
        }

        [TestMethod]
        public void returns_false_when_the_value_is_not_equal_to_the_input()
        {
            var field = new Field(System.Text.Encoding.ASCII.GetBytes("34=20170423-12:32:23.123\u0001"), 0, new Segment(3, 21), 25, 0);

            field.Is(DateTime.Parse("27/03/2017 09:34:21.456")).Should().Be(false);
        }

        [TestMethod]
        public void returns_true_when_the_value_is_equal_to_the_input()
        {
            var field = new Field(System.Text.Encoding.ASCII.GetBytes("34=20170327-09:34:21.456\u0001"), 0, new Segment(3, 21), 25, 0);

            field.Is(DateTime.Parse("27/03/2017 09:34:21.456")).Should().Be(true);
        }
    }
}