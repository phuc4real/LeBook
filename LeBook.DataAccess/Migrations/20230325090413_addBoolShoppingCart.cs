using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.Migrations
{
    public partial class addBoolShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "toBuy",
                table: "ShoppingCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "toBuy",
                table: "ShoppingCarts");
        }
    }
}
