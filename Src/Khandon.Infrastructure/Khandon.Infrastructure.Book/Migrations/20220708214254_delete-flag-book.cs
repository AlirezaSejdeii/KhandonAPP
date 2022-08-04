using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khandon.Infrastructure.Book.Migrations
{
    public partial class deleteflagbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Flag",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
