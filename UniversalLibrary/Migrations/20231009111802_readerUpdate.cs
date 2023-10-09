using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversalLibrary.Migrations
{
    public partial class readerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Readers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Readers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_UserId",
                table: "Readers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_AspNetUsers_UserId",
                table: "Readers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_AspNetUsers_UserId",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_UserId",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Readers");
        }
    }
}
