using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemMaster.Server.Migrations
{
    public partial class UpdateMaxLengthSAP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SAP",
                table: "PODataUpload",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SAP",
                table: "PODataUpload",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
