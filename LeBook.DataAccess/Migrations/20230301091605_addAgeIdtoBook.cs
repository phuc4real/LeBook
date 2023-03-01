using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.Migrations
{
    public partial class addAgeIdtoBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgeId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_AgeId",
                table: "Books",
                column: "AgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Ages_AgeId",
                table: "Books",
                column: "AgeId",
                principalTable: "Ages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Ages_AgeId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_AgeId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AgeId",
                table: "Books");
        }
    }
}
