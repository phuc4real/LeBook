using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeBook.Migrations
{
    public partial class edtUserAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "UserAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "UserAddresses");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "UserAddresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
