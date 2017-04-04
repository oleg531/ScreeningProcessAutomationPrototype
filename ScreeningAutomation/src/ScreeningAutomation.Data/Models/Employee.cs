namespace ScreeningAutomation.Data.Models
{
    using Base;
    public class Employee : BaseEntity
    {
        public string Alias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
