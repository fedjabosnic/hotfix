using System;
using FluentAssertions;
using HotFix.Core;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Test.utilities.scheduling
{
    [TestClass]
    public class is_active_at
    {
        [TestMethod]
        public void test1()
        {
            // | M | T | W | T | F | S | S |
            // | |-------X-------|         |

            new Schedule
            {
                OpenDay = DayOfWeek.Monday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = DayOfWeek.Friday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 12:00:00")) // Wednesday
            .Should().Be(true);
        }

        [TestMethod]
        public void test2()
        {
            // | M | T | W | T | F | S | S |
            // | |---------------|   X     |

            new Schedule
            {
                OpenDay = DayOfWeek.Monday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = DayOfWeek.Friday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/07/01 12:00:00")) // Saturday
            .Should().Be(false);
        }

        [TestMethod]
        public void test3()
        {
            // | M | T | W | T | F | S | S |
            // |-|               |---X-----|

            new Schedule
            {
                OpenDay = DayOfWeek.Friday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = DayOfWeek.Monday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/07/01 12:00:00")) //Saturday
            .Should().Be(true);
        }

        [TestMethod]
        public void test4()
        {
            // | M | T | W | T | F | S | S |
            // |-|       X       |---------|

            new Schedule
            {
                OpenDay = DayOfWeek.Friday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = DayOfWeek.Monday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 12:00:00")) //Wednesday
            .Should().Be(false);
        }

        [TestMethod]
        public void test5()
        {
            // - 10:00:00
            // |
            // |
            // X 12:00:00
            // |
            // |
            // - 14:00:00

            new Schedule
            {
                OpenDay = DayOfWeek.Wednesday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = DayOfWeek.Wednesday,
                CloseTime = TimeSpan.Parse("14:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 12:00:00")) //Wednesday
            .Should().Be(true);
        }

        [TestMethod]
        public void test6()
        {
            // - 10:00:00
            // |
            // |
            // - 12:00:00
            // 
            // 
            // X 14:00:00

            new Schedule
            {
                OpenDay = DayOfWeek.Wednesday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = DayOfWeek.Wednesday,
                CloseTime = TimeSpan.Parse("12:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 14:00:00")) //Wednesday
            .Should().Be(false);
        }

        [TestMethod]
        public void test7()
        {
            // X 10:00:00
            // 
            // 
            // - 12:00:00
            // |
            // |
            // - 14:00:00

            new Schedule
            {
                OpenDay = DayOfWeek.Wednesday,
                OpenTime = TimeSpan.Parse("12:00:00"),

                CloseDay = DayOfWeek.Wednesday,
                CloseTime = TimeSpan.Parse("14:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 10:00:00")) //Wednesday
            .Should().Be(false);
        }

        [TestMethod]
        public void test8()
        {
            // |
            // |
            // - 10:00:00
            // 
            // 
            // X 12:00:00
            // 
            // 
            // - 14:00:00
            // |
            // |

            new Schedule
            {
                OpenDay = DayOfWeek.Wednesday,
                OpenTime = TimeSpan.Parse("14:00:00"),

                CloseDay = DayOfWeek.Wednesday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 12:00:00")) //Wednesday
            .Should().Be(false);
        }

        [TestMethod]
        public void test9()
        {
            // |
            // |
            // X 10:00:00
            // |
            // |
            // - 12:00:00
            // 
            // 
            // - 14:00:00
            // |
            // |

            new Schedule
            {
                OpenDay = DayOfWeek.Wednesday,
                OpenTime = TimeSpan.Parse("12:00:00"),

                CloseDay = DayOfWeek.Wednesday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 10:00:00")) //Wednesday
            .Should().Be(true);
        }

        [TestMethod]
        public void test10()
        {
            // |
            // |
            // - 10:00:00
            // 
            // 
            // - 12:00:00
            // |
            // |
            // X 14:00:00
            // |
            // |

            new Schedule
            {
                OpenDay = DayOfWeek.Wednesday,
                OpenTime = TimeSpan.Parse("12:00:00"),

                CloseDay = DayOfWeek.Wednesday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .IsActiveAt(DateTime.Parse("2017/06/28 14:00:00")) //Wednesday
            .Should().Be(true);
        }
    }

}
