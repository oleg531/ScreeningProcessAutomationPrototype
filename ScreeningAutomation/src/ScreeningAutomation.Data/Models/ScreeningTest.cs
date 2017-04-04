namespace ScreeningAutomation.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Base;


    public class ScreeningTest : BaseEntity
    {
        public string Name { get; set; }
        public TimeSpan ValidPeriod { get; set; }

        public virtual List<ScreeningTestPassingActive> ScreeningTestPassingActive { get; set; }
        public virtual List<ScreeningTestPassedHistory> ScreeningTestPassedHistory { get; set; }
        public virtual List<ScreeningTestPassingPlan> ScreeningTestPassingPlan { get; set; }
    }
}
