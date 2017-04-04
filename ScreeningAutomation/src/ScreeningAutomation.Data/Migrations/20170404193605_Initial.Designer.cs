using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ScreeningAutomation.Data;
using ScreeningAutomation.Data.Models;

namespace ScreeningAutomation.Data.Migrations
{
    [DbContext(typeof(ScreeningAutomationDbContext))]
    [Migration("20170404193605_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ScreeningAutomation.Data.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .HasMaxLength(200);

                    b.Property<string>("LastName")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<TimeSpan>("ValidPeriod");

                    b.HasKey("Id");

                    b.ToTable("ScreeningTest");
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTestPassedHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DatePass");

                    b.Property<int>("EmployeeId");

                    b.Property<int>("ScreeningTestId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ScreeningTestId");

                    b.ToTable("ScreeningTestPassedHistory");
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTestPassingActive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DatePass");

                    b.Property<int>("EmployeeId");

                    b.Property<int>("ScreeningTestId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ScreeningTestId");

                    b.ToTable("ScreeningTestPassingActive");
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTestPassingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeId");

                    b.Property<int>("ScreeningTestId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ScreeningTestId");

                    b.ToTable("ScreeningTestPassingPlan");
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTestPassedHistory", b =>
                {
                    b.HasOne("ScreeningAutomation.Data.Models.Employee", "Employee")
                        .WithMany("ScreeningTestPassedHistory")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScreeningAutomation.Data.Models.ScreeningTest", "ScreeningTest")
                        .WithMany("ScreeningTestPassedHistory")
                        .HasForeignKey("ScreeningTestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTestPassingActive", b =>
                {
                    b.HasOne("ScreeningAutomation.Data.Models.Employee", "Employee")
                        .WithMany("ScreeningTestPassingActive")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScreeningAutomation.Data.Models.ScreeningTest", "ScreeningTest")
                        .WithMany("ScreeningTestPassingActive")
                        .HasForeignKey("ScreeningTestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScreeningAutomation.Data.Models.ScreeningTestPassingPlan", b =>
                {
                    b.HasOne("ScreeningAutomation.Data.Models.Employee", "Employee")
                        .WithMany("ScreeningTestPassingPlan")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ScreeningAutomation.Data.Models.ScreeningTest", "ScreeningTest")
                        .WithMany("ScreeningTestPassingPlan")
                        .HasForeignKey("ScreeningTestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
