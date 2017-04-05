namespace ScreeningAutomation.API.Services
{
    using System.Threading.Tasks;

    public interface IScreeningStatusMonitoringService
    {
        Task CheckScreeningsStatus();
    }
}
