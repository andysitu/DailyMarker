using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyMarker.Data.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    TableTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTasks_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    TableTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyTasks_TableTasks_TableTaskId",
                        column: x => x.TableTaskId,
                        principalTable: "TableTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyTask_TaskDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyTaskId = table.Column<int>(nullable: false),
                    TaskDateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTask_TaskDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyTask_TaskDates_DailyTasks_DailyTaskId",
                        column: x => x.DailyTaskId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyTask_TaskDates_TaskDates_TaskDateId",
                        column: x => x.TaskDateId,
                        principalTable: "TaskDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyTask_TaskDates_DailyTaskId",
                table: "DailyTask_TaskDates",
                column: "DailyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTask_TaskDates_TaskDateId",
                table: "DailyTask_TaskDates",
                column: "TaskDateId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTasks_TableTaskId",
                table: "DailyTasks",
                column: "TableTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTasks_UserAccountId",
                table: "TableTasks",
                column: "UserAccountId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTask_TaskDates");

            migrationBuilder.DropTable(
                name: "DailyTasks");

            migrationBuilder.DropTable(
                name: "TaskDates");

            migrationBuilder.DropTable(
                name: "TableTasks");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
