﻿namespace ScreeningAutomation.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    // For migrations only
    public class DbContextFactory : IDbContextFactory<ScreeningAutomationDbContext>
    {
        public ScreeningAutomationDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScreeningAutomationDbContext>();
            optionsBuilder.UseSqlServer("Server=screeningautomation-server.database.windows.net,1433;Database=ScreeningAutomation;User ID=ccPortal;Password=*2l807Eh*qB>B*U;MultipleActiveResultSets=true");

            return new ScreeningAutomationDbContext(optionsBuilder.Options);
        }
    }
}
