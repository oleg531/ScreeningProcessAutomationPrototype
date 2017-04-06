namespace ScreeningAutomation.API
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;    
    using Data;
    using Data.Models;
    using Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Options;
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
            services.AddOptions();
            services.Configure<EmailSenderOptions>(emailSenderOptions =>
            {
                emailSenderOptions.Server = Configuration["email:server"];
                int port;
                if (!int.TryParse(Configuration["email:port"], out port))
                {
                    // TODO change exception type
                    throw new ArgumentException(
                        "Email sender: port variable contains incorrect value. Check configuration file.");
                }
                emailSenderOptions.Port = port;
                emailSenderOptions.Credentials = new EmailCredentials
                {
                    Address = Configuration["email:emailCredentials:address"],
                    Password = Configuration["email:emailCredentials:password"]
                };
            });

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
            services.AddScoped<IEmailSender, EmailSender>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            //CORS            
            app.UseCors(builder =>
                builder.WithOrigins(Configuration["FrontEndSiteBaseAddress"])
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseMvc();

            DataSeeder.SeedData(app);
        }
    }
}
