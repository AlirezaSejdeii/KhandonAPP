using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khandon.Infrastructure.Book.Migrations
{
    public partial class editbook_edit_userId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserdId",
                table: "Books",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Books",
                newName: "UserdId");
        }
    }
}
