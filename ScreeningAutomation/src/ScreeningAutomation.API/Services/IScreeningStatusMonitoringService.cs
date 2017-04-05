namespace ScreeningAutomation.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTO;

    public interface IScreeningStatusMonitoringService
    {
        Task CheckScreeningsStatus();
        Task<IEnumerable<EmployeeScreeningDto>> GetEmployeeScreenings();
    }
}
