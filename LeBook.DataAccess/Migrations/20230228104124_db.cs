using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Aages",
                table: "Aages");

            migrationBuilder.RenameTable(
                name: "Aages",
                newName: "Ages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ages",
                table: "Ages",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ages",
                table: "Ages");

            migrationBuilder.RenameTable(
                name: "Ages",
                newName: "Aages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aages",
                table: "Aages",
                column: "Id");
        }
    }
}
