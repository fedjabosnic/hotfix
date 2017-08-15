using System;

namespace HotFix.Core
{
    internal class ActiveSchedule
    {
        public string Name { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }

        public override string ToString() => $"{Name}: {Open} - {Close}";
    }
}
