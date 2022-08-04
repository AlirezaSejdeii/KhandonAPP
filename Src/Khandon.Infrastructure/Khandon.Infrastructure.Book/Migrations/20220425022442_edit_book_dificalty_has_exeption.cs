using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khandon.Infrastructure.Book.Migrations
{
    public partial class edit_book_dificalty_has_exeption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyLe",
                table: "Books");

            migrationBuilder.AddColumn<byte>(
                name: "Difficultye",
                table: "Books",
                type: "tinyint",
                maxLength: 10,
                nullable: false,
                defaultValue: (byte)1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficultye",
                table: "Books");

            migrationBuilder.AddColumn<byte>(
                name: "DifficultyLe",
                table: "Books",
                type: "tinyint",
                maxLength: 3,
                nullable: false,
                defaultValue: (byte)1);
        }
    }
}
