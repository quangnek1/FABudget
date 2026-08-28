using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemMaster.Server.Migrations
{
    public partial class PODataUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PODataUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PR = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    PR_No = table.Column<int>(type: "int", nullable: true),
                    PO = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    Item = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AmountInLocalCurrency = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LocalCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LaborCost = table.Column<int>(type: "int", nullable: true),
                    Ring = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SAP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PODataUpload", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PODataUpload");
        }
    }
}
