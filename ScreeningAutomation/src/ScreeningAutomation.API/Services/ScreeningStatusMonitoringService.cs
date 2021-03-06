﻿namespace ScreeningAutomation.API.Services
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
        private readonly IRepository<ScreeningTestPassedHistory> _screeningTestPassedHistoryRepository;
        private readonly IEmailSender _emailSender;

        public ScreeningStatusMonitoringService(
            IRepository<ScreeningTestPassingActive> screeningTestPassingActiveRepository,
            IRepository<ScreeningTestPassingPlan> screeningTestPassingPlanRepository,
            IRepository<ScreeningTestPassedHistory> screeningTestPassedHistoryRepository,
            IEmailSender emailSender
        )
        {
            _screeningTestPassingActiveRepository = screeningTestPassingActiveRepository;
            _screeningTestPassingPlanRepository = screeningTestPassingPlanRepository;
            _screeningTestPassedHistoryRepository = screeningTestPassedHistoryRepository;
            _emailSender = emailSender;
        }

        public async Task CheckScreeningsStatus(string commonEmail)
        {
            var notPassedTests = await GetScreeningsForEmployee()
                .Where(testsGroup => testsGroup.ScreeningTestPassingActive == null)
                .Select(testsGroup => testsGroup.ScreeningTestPassingPlan)
                .ToListAsync();


            var overdueTests = await _screeningTestPassingActiveRepository.GetAll()
                .Include(activeTest => activeTest.ScreeningTest)
                .Include(activeTest => activeTest.Employee)
                .Where(activeTest => DateTimeOffset.UtcNow >
                                     activeTest.DatePass.Add(activeTest.ScreeningTest.ValidPeriod))
                .ToListAsync();

            var notPassedRecipients = notPassedTests.GroupBy(notPassedTest => notPassedTest.Employee.Email).ToList();
            var overdueRecipients = overdueTests.GroupBy(overdue => overdue.Employee.Email).ToList();

            var subjectBase = " screening tests it's needed to be passed";

            var notPassedEmailTasks =
                notPassedRecipients.Select(r => Task.Run(
                        () => _emailSender.SendEmailAsync(r.Key, $"New {subjectBase}",
                            string.Join(", ", r.Select(x => x.ScreeningTest.Name)))))
                    .ToList();
            var overdueTasks = overdueRecipients.Select(r => Task.Run(
                    () => _emailSender.SendEmailAsync(r.Key, $"Overdue {subjectBase}",
                        string.Join(", ", r.Select(x => x.ScreeningTest.Name)))))
                .ToList();

            
            var commonEmailTask = Task.Run(() =>
            {
                var messageNotPassed = string.Join(" Email: ",
                    notPassedRecipients.Select(
                        x => $"{x.Key} Tests: {string.Join(",", x.Select(y => y.ScreeningTest.Name))}"));
                var messageOverdue = string.Join(" Email: ",
                    overdueRecipients.Select(
                        x => $"{x.Key} Tests: {string.Join(",", x.Select(y => y.ScreeningTest.Name))}"));
                _emailSender.SendEmailAsync(commonEmail, "Screening tests for all Employees",
                    $"New {subjectBase} : {messageNotPassed};  Overdue {subjectBase} : {messageOverdue}");
            });

            await Task.WhenAll(notPassedEmailTasks.Concat(overdueTasks));
            await commonEmailTask;
        }

        public async Task<IEnumerable<EmployeeScreeningDto>> GetEmployeeScreenings()
        {
            var screeningTests = await GetScreeningsForEmployee().ToListAsync();

            return screeningTests
                .Select(screeningTest => new EmployeeScreeningDto
                {
                    ActiveTestId = screeningTest.ScreeningTestPassingActive?.Id,
                    Email = screeningTest.ScreeningTestPassingPlan.Employee.Email,
                    Alias = screeningTest.ScreeningTestPassingPlan.Employee.Alias,
                    ScreeningTestName = screeningTest.ScreeningTestPassingPlan.ScreeningTest.Name,
                    DatePass = screeningTest.ScreeningTestPassingActive?.DatePass,
                    ExpirationDate = screeningTest.ScreeningTestPassingActive?.DatePass.Add(screeningTest
                        .ScreeningTestPassingPlan.ScreeningTest.ValidPeriod),
                    Status = screeningTest.ScreeningTestPassingActive == null
                        ? ScreeningTestPassingStatus.NotPassed.ToString()
                        : DateTimeOffset.UtcNow >
                          screeningTest.ScreeningTestPassingActive.DatePass.Add(screeningTest
                              .ScreeningTestPassingPlan.ScreeningTest.ValidPeriod)
                            ? ScreeningTestPassingStatus.Overdue.ToString()
                            : ScreeningTestPassingStatus.Valid.ToString()
                });
        }

        public async Task PassTScreeningTest(int activeTestId)
        {
            var screeningTest = await _screeningTestPassingActiveRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == activeTestId);

            if (screeningTest == null)
            {
                throw new KeyNotFoundException($"Could not find active test with id={activeTestId}");
            }

            using (var transaction = _screeningTestPassedHistoryRepository.BeginTransaction())
            {
                try
                {
                    await _screeningTestPassedHistoryRepository.SaveAsync(new ScreeningTestPassedHistory
                    {
                        DatePass = DateTimeOffset.UtcNow,
                        CreatedDate = DateTimeOffset.UtcNow,
                        EmployeeId = screeningTest.EmployeeId,
                        ScreeningTestId = screeningTest.ScreeningTestId
                    });

                    screeningTest.DatePass = DateTimeOffset.UtcNow;
                    await _screeningTestPassingActiveRepository.SaveAsync(screeningTest);


                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private IQueryable<ScreeningTestProxy> GetScreeningsForEmployee()
        {
            return _screeningTestPassingPlanRepository.GetAll()
                .Include(planningTest => planningTest.Employee)
                .Include(planningTest => planningTest.ScreeningTest)
                .GroupJoin(_screeningTestPassingActiveRepository.GetAll(),
                    plannedTest => new {plannedTest.EmployeeId, plannedTest.ScreeningTestId},
                    activeTest => new {activeTest.EmployeeId, activeTest.ScreeningTestId},
                    (plannedTest, activeTestCollection) => new {plannedTest, activeTestCollection})
                .SelectMany(group => group.activeTestCollection.DefaultIfEmpty(),
                    (group, activeTest) => new ScreeningTestProxy
                    {
                        ScreeningTestPassingPlan = group.plannedTest,
                        ScreeningTestPassingActive = activeTest
                    });
        }


    }

    internal class ScreeningTestProxy
    {
        public ScreeningTestPassingPlan ScreeningTestPassingPlan { get; set; }
        public ScreeningTestPassingActive ScreeningTestPassingActive { get; set; }
    }
}
