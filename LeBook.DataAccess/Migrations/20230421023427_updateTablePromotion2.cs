using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.DataAccess.Migrations
{
    public partial class updateTablePromotion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PromotionDecription",
                table: "Promotion",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Promotion",
                newName: "PromotionDecription");
        }
    }
}
