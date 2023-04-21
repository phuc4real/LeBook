using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.DataAccess.Migrations
{
    public partial class updateTablePromotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PromotionBanner",
                table: "Promotion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionBanner",
                table: "Promotion");
        }
    }
}
