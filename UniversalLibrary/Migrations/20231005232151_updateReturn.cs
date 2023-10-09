using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversalLibrary.Migrations
{
    public partial class updateReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pdf",
                table: "ReturnBooks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pdf",
                table: "ReturnBooks");
        }
    }
}
