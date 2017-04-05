namespace ScreeningAutomation.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using Data.Repositories;
    using DTO;
    using Microsoft.EntityFrameworkCore;

    public class ScreeningStatusMonitoringService : IScreeningStatusMonitoringService
    {
        private readonly IRepository<ScreeningTestPassingActive> _screeningTestPassingActiveRepository;
        private readonly IRepository<ScreeningTestPassingPlan> _screeningTestPassingPlanRepository;
        private readonly IEmailSender _emailSender;

        public ScreeningStatusMonitoringService(
            IRepository<ScreeningTestPassingActive> screeningTestPassingActiveRepository,
            IRepository<ScreeningTestPassingPlan> screeningTestPassingPlanRepository,
            IEmailSender emailSender
        )
        {
            _screeningTestPassingActiveRepository = screeningTestPassingActiveRepository;
            _screeningTestPassingPlanRepository = screeningTestPassingPlanRepository;
            _emailSender = emailSender;
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

            var notPassedRecipients = notPassedTests.GroupBy(notPassedTest => notPassedTest.Employee.Email);
            var overdueRecipients = overdueTests.GroupBy(overdue => overdue.Employee.Email);

            var subjectBase = " screening tests it's needed to be passed";
            var notPassedEmailTasks =
                notPassedRecipients.Select(r => Task.Run(
                    () => _emailSender.SendEmailAsync(r.Key, $"New {subjectBase}",
                        string.Join(", ", r.Select(x => x.ScreeningTest.Name)))));
            var overdueTasks = overdueRecipients.Select(r => Task.Run(
                () => _emailSender.SendEmailAsync(r.Key, $"Overdue {subjectBase}",
                    string.Join(", ", r.Select(x => x.ScreeningTest.Name)))));

            await Task.WhenAll(notPassedEmailTasks.Concat(overdueTasks));
        }

        public async Task<IEnumerable<EmployeeScreeningDto>> GetEmployeeScreenings()
        {
            // todo add not passed tests
            return await _screeningTestPassingActiveRepository.GetAll()
                .Include(activeTest => activeTest.Employee)
                .Include(activeTest => activeTest.ScreeningTest)
                .Select(activeTest => new EmployeeScreeningDto
                {
                    Email = activeTest.Employee.Email,
                    Alias = activeTest.Employee.Alias,
                    ScreeningTestName = activeTest.ScreeningTest.Name,
                    DatePass = activeTest.DatePass,
                    ExpirationDate = activeTest.DatePass.Add(activeTest.ScreeningTest.ValidPeriod),
                    Status = DateTimeOffset.UtcNow > activeTest.DatePass.Add(activeTest.ScreeningTest.ValidPeriod)
                        ? ScreeningTestPassingStatus.Overdue.ToString()
                        : ScreeningTestPassingStatus.Valid.ToString()
                })
                .ToListAsync();
        }
    }
}
