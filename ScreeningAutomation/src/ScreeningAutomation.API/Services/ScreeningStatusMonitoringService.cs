namespace ScreeningAutomation.API.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using Data.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class ScreeningStatusMonitoringService : IScreeningStatusMonitoringService
    {
        private readonly IRepository<ScreeningTestPassingActive> _screeningTestPassingActiveRepository;
        private readonly IRepository<ScreeningTestPassingPlan> _screeningTestPassingPlanRepository;

        public ScreeningStatusMonitoringService(
            IRepository<ScreeningTestPassingActive> screeningTestPassingActiveRepository,
            IRepository<ScreeningTestPassingPlan> screeningTestPassingPlanRepository
            )
        {
            _screeningTestPassingActiveRepository = screeningTestPassingActiveRepository;
            _screeningTestPassingPlanRepository = screeningTestPassingPlanRepository;
        }

        public async Task CheckScreeningsStatus()
        {
            var notPassedTests = await _screeningTestPassingPlanRepository.GetAll()
                .Include(planningTest => planningTest.Employee)
                .Where(planningTest => _screeningTestPassingActiveRepository.GetAll()
                    .All(activeTest => activeTest.ScreeningTestId != planningTest.ScreeningTestId))
                .ToListAsync();
            
            
            var overdueTests = await _screeningTestPassingActiveRepository.GetAll()
                .Include(activeTest => activeTest.ScreeningTest)
                .Include(activeTest => activeTest.Employee)
                .Where(activeTest => DateTimeOffset.UtcNow >
                                     activeTest.DatePass.Add(activeTest.ScreeningTest.ValidPeriod))
                .ToListAsync();

            // TODO to send emails about this tests
        }
    }
}
