using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ScreeningAutomation.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 200, nullable: true),
                    LastName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    ValidPeriod = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningTestPassedHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatePass = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    ScreeningTestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningTestPassedHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreeningTestPassedHistory_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreeningTestPassedHistory_ScreeningTest_ScreeningTestId",
                        column: x => x.ScreeningTestId,
                        principalTable: "ScreeningTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningTestPassingActive",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatePass = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    ScreeningTestId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningTestPassingActive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreeningTestPassingActive_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreeningTestPassingActive_ScreeningTest_ScreeningTestId",
                        column: x => x.ScreeningTestId,
                        principalTable: "ScreeningTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningTestPassingPlan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    ScreeningTestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningTestPassingPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreeningTestPassingPlan_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreeningTestPassingPlan_ScreeningTest_ScreeningTestId",
                        column: x => x.ScreeningTestId,
                        principalTable: "ScreeningTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningTestPassedHistory_EmployeeId",
                table: "ScreeningTestPassedHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningTestPassedHistory_ScreeningTestId",
                table: "ScreeningTestPassedHistory",
                column: "ScreeningTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningTestPassingActive_EmployeeId",
                table: "ScreeningTestPassingActive",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningTestPassingActive_ScreeningTestId",
                table: "ScreeningTestPassingActive",
                column: "ScreeningTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningTestPassingPlan_EmployeeId",
                table: "ScreeningTestPassingPlan",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningTestPassingPlan_ScreeningTestId",
                table: "ScreeningTestPassingPlan",
                column: "ScreeningTestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScreeningTestPassedHistory");

            migrationBuilder.DropTable(
                name: "ScreeningTestPassingActive");

            migrationBuilder.DropTable(
                name: "ScreeningTestPassingPlan");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "ScreeningTest");
        }
    }
}
