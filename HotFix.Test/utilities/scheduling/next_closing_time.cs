using System;
using FluentAssertions;
using HotFix.Core;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.DayOfWeek;

namespace HotFix.Test.utilities.scheduling
{
    [TestClass]
    public class next_closing_time
    {
        [TestMethod]
        public void test1()
        {
            // | M | T | W | T | F | S | S |
            // | |-------X-------|         |

            new Schedule
            {
                OpenDay = Monday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = Friday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) // Wednesday
            .Should().Be(DateTime.ParseExact("30/06/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test2()
        {
            // | M | T | W | T | F | S | S |
            // | |---------------|   X     |

            new Schedule
            {
                OpenDay = Monday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = Friday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("01/07/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) // Saturday
            .Should().Be(DateTime.ParseExact("07/07/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test3()
        {
            // | M | T | W | T | F | S | S |
            // |-|               |---X-----|

            new Schedule
            {
                OpenDay = Friday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = Monday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("01/07/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Saturday
            .Should().Be(DateTime.ParseExact("03/07/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }

        [TestMethod]
        public void test4()
        {
            // | M | T | W | T | F | S | S |
            // |-|       X       |---------|

            new Schedule
            {
                OpenDay = Friday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = Monday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("03/07/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
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
                OpenDay = Wednesday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = Wednesday,
                CloseTime = TimeSpan.Parse("14:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("28/06/2017 14:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
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
                OpenDay = Wednesday,
                OpenTime = TimeSpan.Parse("10:00:00"),

                CloseDay = Wednesday,
                CloseTime = TimeSpan.Parse("12:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 14:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("05/07/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
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
                OpenDay = Wednesday,
                OpenTime = TimeSpan.Parse("12:00:00"),

                CloseDay = Wednesday,
                CloseTime = TimeSpan.Parse("14:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("28/06/2017 14:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
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
                OpenDay = Wednesday,
                OpenTime = TimeSpan.Parse("14:00:00"),

                CloseDay = Wednesday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("05/07/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
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
                OpenDay = Wednesday,
                OpenTime = TimeSpan.Parse("14:00:00"),

                CloseDay = Wednesday,
                CloseTime = TimeSpan.Parse("12:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
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
                OpenDay = Wednesday,
                OpenTime = TimeSpan.Parse("12:00:00"),

                CloseDay = Wednesday,
                CloseTime = TimeSpan.Parse("10:00:00")
            }
            .NextClosingTime(DateTime.ParseExact("28/06/2017 14:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
            .Should().Be(DateTime.ParseExact("05/07/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null));
        }
    }
}
