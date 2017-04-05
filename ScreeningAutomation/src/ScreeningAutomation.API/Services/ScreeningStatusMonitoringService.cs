namespace ScreeningAutomation.API.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Data.Models;
    using Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Remotion.Linq.Clauses;

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
                .Include(planningTest => planningTest.ScreeningTest)
                .GroupJoin(_screeningTestPassingActiveRepository.GetAll(),
                    plannedTest => new {plannedTest.EmployeeId, plannedTest.ScreeningTestId},
                    activeTest => new {activeTest.EmployeeId, activeTest.ScreeningTestId},
                    (plannedTest, activeTestCollection) => new {plannedTest, activeTestCollection})
                .SelectMany(group => group.activeTestCollection.DefaultIfEmpty(),
                    (group, activeTest) => new {PlanedTest = group.plannedTest, ActiveTest = activeTest})
                .Where(testsGroup => testsGroup.ActiveTest == null)
                .Select(testsGroup => testsGroup.PlanedTest)
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
