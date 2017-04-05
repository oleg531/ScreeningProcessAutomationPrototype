using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ScreeningAutomation.API
{
    using Data;
    using Data.Models;
    using Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Services;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ScreeningAutomationDbContext>(
                options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            
            // add repository
            services.AddScoped<IRepository<Employee>, Repository<Employee>>();
            services.AddScoped<IRepository<ScreeningTest>, Repository<ScreeningTest>>();
            services.AddScoped<IRepository<ScreeningTestPassedHistory>, Repository<ScreeningTestPassedHistory>>();
            services.AddScoped<IRepository<ScreeningTestPassingActive>, Repository<ScreeningTestPassingActive>>();
            services.AddScoped<IRepository<ScreeningTestPassingPlan>, Repository<ScreeningTestPassingPlan>>();        
            
            // add services
            services.AddScoped<IScreeningStatusMonitoringService, ScreeningStatusMonitoringService>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
