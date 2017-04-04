namespace ScreeningAutomation.Data.Models
{
    using System;
    using Base;

    public class ScreeningTestPassingActive : BaseEntity
    {
        public int ImployeeId { get; set; }
        public int ScreeningTestId { get; set; }
        public ScreeningTestPassingStatus Status { get; set; }
        public DateTimeOffset DatePass { get; set; }
    }
}
