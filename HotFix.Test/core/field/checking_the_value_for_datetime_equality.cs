﻿using System;
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
            var field = new FIXField("34=XXXXXX-YY:YY:YY.ZZZ\u0001".AsBytes(), 0, new Segment(3, 21), 25, 0);

            field.Is("2017/03/27 09:34:21.456".AsDateTime()).Should().Be(false);
        }

        [TestMethod]
        public void returns_false_when_the_value_is_not_equal_to_the_input()
        {
            var field = new FIXField("34=20170423-12:32:23.123\u0001".AsBytes(), 0, new Segment(3, 21), 25, 0);

            field.Is("2017/03/27 09:34:21.456".AsDateTime()).Should().Be(false);
        }

        [TestMethod]
        public void returns_true_when_the_value_is_equal_to_the_input()
        {
            var field = new FIXField("34=20170327-09:34:21.456\u0001".AsBytes(), 0, new Segment(3, 21), 25, 0);

            field.Is("2017/03/27 09:34:21.456".AsDateTime()).Should().Be(true);
        }
    }
}