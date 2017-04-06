namespace ScreeningAutomation.API.Tests.Services
{
    using System.Threading.Tasks;
    using API.Services;
    using Data;
    using Data.Models;
    using Data.Repositories;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Options;

    [TestClass]
    public class ScreeningStatusMonitoringServiceTest
    {
        [TestMethod]
        public async Task CheckScreeningsStatus_ShouldWork()
        {
            var context = new DbContextFactory().Create(null);
            var screeningTestPassingActiveRepository = new Repository<ScreeningTestPassingActive>(context);
            var screeningTestPassingPlanRepository = new Repository<ScreeningTestPassingPlan>(context);

            // TODO move to common config
            var emailService = new EmailSender(new OptionsWrapper<EmailSenderOptions>(new EmailSenderOptions
            {
                Server = "smtp.gmail.com",
                Port = 587,
                Credentials = new EmailCredentials
                {
                    Address = "EmailSenderTesting.123",
                    Password = "9){fmJ#kf,72G[Cm"
                }
            }));
            var service = new ScreeningStatusMonitoringService(screeningTestPassingActiveRepository,
                screeningTestPassingPlanRepository, emailService);

            await service.CheckScreeningsStatus("oleg.lazarenko@akvelon.com");
        }
    }
}
