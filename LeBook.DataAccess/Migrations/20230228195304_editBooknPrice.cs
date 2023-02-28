using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.Migrations
{
    public partial class editBooknPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Prices_PriceId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PriceId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Prices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_BookId",
                table: "Prices",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Books_BookId",
                table: "Prices",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Books_BookId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_BookId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Prices");

            migrationBuilder.AddColumn<int>(
                name: "PriceId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PriceId",
                table: "Books",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Prices_PriceId",
                table: "Books",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
