namespace ScreeningAutomation.Data.Models
{
    using System;
    using Base;


    public class ScreeningTest : BaseEntity
    {
        public string Name { get; set; }
        public TimeSpan ValidPeriod { get; set; }
    }
}
