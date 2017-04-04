namespace ScreeningAutomation.Data.Models
{
    using System.Collections.Generic;
    using Base;
    public class Employee : BaseEntity
    {
        public string Alias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<ScreeningTestPassingActive> ScreeningTestPassingActive { get; set; }
        public virtual List<ScreeningTestPassedHistory> ScreeningTestPassedHistory { get; set; }
        public virtual List<ScreeningTestPassingPlan> ScreeningTestPassingPlan { get; set; }        
    }
}
