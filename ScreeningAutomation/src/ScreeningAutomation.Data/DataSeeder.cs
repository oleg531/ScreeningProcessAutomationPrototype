﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScreeningAutomation.Data
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Models;

    public static class DataSeeder
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            var dbContext =
                (ScreeningAutomationDbContext) applicationBuilder.ApplicationServices.GetService(
                    typeof(ScreeningAutomationDbContext));

            if (!dbContext.Employee.Any())
            {
                var employee = new Employee
                {
                    Alias = "testAlias",
                    FirstName = "Some first name",
                    LastName = "Some last name",
                    Email = "oleg.lazarenko@akvelon.com",
                    CreatedDate = DateTimeOffset.UtcNow,
                    ScreeningTestPassingPlan = new List<ScreeningTestPassingPlan>
                    {
                        new ScreeningTestPassingPlan
                        {
                            ScreeningTest = new ScreeningTest
                            {
                                Name = "Screening test 1",
                                ValidPeriod = TimeSpan.FromMinutes(5),
                                CreatedDate = DateTimeOffset.UtcNow
                            },
                            CreatedDate = DateTimeOffset.UtcNow
                        },
                        new ScreeningTestPassingPlan
                        {
                            ScreeningTest = new ScreeningTest
                            {
                                Name = "Screening test 2",
                                ValidPeriod = TimeSpan.FromMinutes(5),
                                CreatedDate = DateTimeOffset.UtcNow
                            },
                            CreatedDate = DateTimeOffset.UtcNow
                        }
                    }
                };

                dbContext.Add(employee);

                var anotherEmployee =  new Employee
                {
                    Alias = "testAlias2",
                    FirstName = "Some first name2",
                    LastName = "Some last name2",
                    Email = "oleg.lazarenko@akvelon.com",
                    CreatedDate = DateTimeOffset.UtcNow,
                    ScreeningTestPassingPlan = new List<ScreeningTestPassingPlan>
                    {
                        employee.ScreeningTestPassingPlan[0],
                        new ScreeningTestPassingPlan
                        {
                            ScreeningTest = new ScreeningTest
                            {
                                Name = "Screening test 3",
                                ValidPeriod = TimeSpan.FromMinutes(5),
                                CreatedDate = DateTimeOffset.UtcNow
                            },
                            CreatedDate = DateTimeOffset.UtcNow
                        }
                    },
                    ScreeningTestPassingActive = new List<ScreeningTestPassingActive>
                    {
                        new ScreeningTestPassingActive
                        {
                            ScreeningTestId = employee.ScreeningTestPassingPlan[0].Id,
                            CreatedDate = DateTimeOffset.UtcNow,
                            DatePass = DateTimeOffset.UtcNow,
                            Status = ScreeningTestPassingStatus.Valid
                        }
                    }
                };

                dbContext.Add(anotherEmployee);

                dbContext.SaveChanges();
            }
        }
    }
}
