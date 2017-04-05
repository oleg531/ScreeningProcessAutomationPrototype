namespace ScreeningAutomation.API.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using API.Services;
    using Data;
    using Data.Models;
    using Data.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ScreeningStatusMonitoringServiceTest
    {
        [TestMethod]
        public async Task CheckScreeningsStatus_ShouldWork()
        {
            var context = new DbContextFactory().Create(null);
            var screeningTestPassingActiveRepository = new Repository<ScreeningTestPassingActive>(context);
            var screeningTestPassingPlanRepository = new Repository<ScreeningTestPassingPlan>(context);
            var service = new ScreeningStatusMonitoringService(screeningTestPassingActiveRepository,
                screeningTestPassingPlanRepository);

            await service.CheckScreeningsStatus();
        }
    }
}
