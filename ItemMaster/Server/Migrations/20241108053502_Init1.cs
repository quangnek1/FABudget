using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemMaster.Server.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    GLACCOUNT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.GLACCOUNT);
                });

            migrationBuilder.CreateTable(
                name: "BudgetSharing",
                columns: table => new
                {
                    BudgetNo = table.Column<int>(type: "int", nullable: false),
                    BudgetSharingNo = table.Column<int>(type: "int", nullable: false),
                    BudgetHasShare = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetSharing", x => new { x.BudgetNo, x.BudgetSharingNo });
                });

            migrationBuilder.CreateTable(
                name: "FABudget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<int>(type: "int", nullable: true),
                    GLACCOUNT = table.Column<int>(type: "int", nullable: true),
                    AccountText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PriorityRank = table.Column<int>(type: "int", nullable: true),
                    AssetName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PersonInCharge = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SectionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Segment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SectionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseTimeBudget = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepreciationStartTimeBudget = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseTimeEstimation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepreciationStartTimeEstimation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CurrentInvestmentAmountBudget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PreviousInvestmentAmountBudget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FixedAssetsAccountedAmountBudget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EXRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CurrentInvestmentAmountUSD = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PreviousInvestmentAmountUSD = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FixedAssetsAccountedAmountUSD = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CurrentInvestmentAmountEstimationUSD = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PreviousInvestmentAmountEstimationUSD = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FixedAssetsAccountedAmountEstimationUSD = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RingishoNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RingishoProcess = table.Column<bool>(type: "bit", nullable: true),
                    AppropriatedBudgetNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BudgetRemaining = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FABudget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupEmailSends",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    CountSend = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupEmailSends", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    Currency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Current = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NextYear = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.Currency);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    SectionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Segment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SectionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.SectionCode);
                });

            migrationBuilder.CreateTable(
                name: "FABudgetHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FABudgetId = table.Column<int>(type: "int", nullable: false),
                    No = table.Column<int>(type: "int", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FABudgetHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FABudgetHistory_FABudget_FABudgetId",
                        column: x => x.FABudgetId,
                        principalTable: "FABudget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FABudgetHistory_FABudgetId",
                table: "FABudgetHistory",
                column: "FABudgetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "BudgetSharing");

            migrationBuilder.DropTable(
                name: "FABudgetHistory");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "GroupEmailSends");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "FABudget");
        }
    }
}
