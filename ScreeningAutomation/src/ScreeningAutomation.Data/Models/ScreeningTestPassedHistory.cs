namespace ScreeningAutomation.Data.Models
{
    using System;
    using Base;
    public class ScreeningTestPassedHistory : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int ScreeningTestId { get; set; }
        public DateTimeOffset DatePass { get; set; }

        public Employee Employee { get; set; }
        public ScreeningTest ScreeningTest { get; set; }
    }
}
