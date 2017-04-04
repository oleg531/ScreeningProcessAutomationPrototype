namespace ScreeningAutomation.Data.Models
{
    using Base;
    public class ScreeningTestPassingPlan : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int ScreeningTestId { get; set; }
    }
}
