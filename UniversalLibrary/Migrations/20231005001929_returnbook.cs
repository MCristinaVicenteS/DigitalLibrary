using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversalLibrary.Migrations
{
    public partial class returnbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loanDetailId",
                table: "ReturnBooks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnBooks_loanDetailId",
                table: "ReturnBooks",
                column: "loanDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnBooks_LoanDetails_loanDetailId",
                table: "ReturnBooks",
                column: "loanDetailId",
                principalTable: "LoanDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnBooks_LoanDetails_loanDetailId",
                table: "ReturnBooks");

            migrationBuilder.DropIndex(
                name: "IX_ReturnBooks_loanDetailId",
                table: "ReturnBooks");

            migrationBuilder.DropColumn(
                name: "loanDetailId",
                table: "ReturnBooks");
        }
    }
}
