using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class changeColNameToPhotoName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
