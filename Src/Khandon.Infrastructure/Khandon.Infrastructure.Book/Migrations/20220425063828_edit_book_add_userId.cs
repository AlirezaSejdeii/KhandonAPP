using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khandon.Infrastructure.Book.Migrations
{
    public partial class edit_book_add_userId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserdId",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserdId",
                table: "Books");
        }
    }
}
