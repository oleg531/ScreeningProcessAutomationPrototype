namespace ScreeningAutomation.API.DTO
{
    using System;

    public class EmployeeScreeningDto
    {
        public string Alias { get; set; }
        public string Email { get; set; }
        public string ScreeningTestName { get; set; }
        public DateTimeOffset DatePass { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public string Status { get; set; }
    }
}
