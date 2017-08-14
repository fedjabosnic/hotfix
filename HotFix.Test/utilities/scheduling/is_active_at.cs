using System;
using FluentAssertions;
using HotFix.Core;
using HotFix.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.DayOfWeek;

namespace HotFix.Test.utilities.scheduling
{
    [TestClass]
    public class is_active_at
    {
        [TestClass]
        public class open_day_and_time_is_before_close_day_and_time
        {
            [TestClass]
            public class different_open_and_close_days
            {
                [TestMethod]
                public void datetime_inside_session()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) // Wednesday
                    .Should().Be(true);
                }

                [TestMethod]
                public void datetime_outside_session()
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
                    .IsActiveAt(DateTime.ParseExact("01/07/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) // Saturday
                    .Should().Be(false);
                }
            }

            [TestClass]
            public class same_open_and_close_days
            {
                [TestMethod]
                public void datetime_between_open_and_close_times()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(true);
                }

                [TestMethod]
                public void datetime_after_open_and_close_times()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 14:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(false);
                }

                [TestMethod]
                public void datetime_before_open_and_close_times()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(false);
                }
            }
        }

        [TestClass]
        public class open_day_and_time_is_after_close_day_and_time
        {
            [TestClass]
            public class different_open_and_close_days
            {
                [TestMethod]
                public void datetime_inside_session()
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
                    .IsActiveAt(DateTime.ParseExact("01/07/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Saturday
                    .Should().Be(true);
                }

                [TestMethod]
                public void datetime_outside_session()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(false);
                }
            }

            [TestClass]
            public class same_open_and_close_days
            {
                [TestMethod]
                public void datetime_between_open_and_close_times()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 12:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(false);
                }

                [TestMethod]
                public void datetime_before_open_and_close_times()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 10:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(true);
                }

                [TestMethod]
                public void datetime_after_open_and_close_times()
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
                    .IsActiveAt(DateTime.ParseExact("28/06/2017 14:00:00.000", "dd/MM/yyyy HH:mm:ss.fff", null)) //Wednesday
                    .Should().Be(true);
                }
            }
        }
    }
}
