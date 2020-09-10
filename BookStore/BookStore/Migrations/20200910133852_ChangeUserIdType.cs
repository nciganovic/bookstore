using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class ChangeUserIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookUser_AspNetUsers_UserId1",
                table: "BookUser");

            migrationBuilder.DropIndex(
                name: "IX_BookUser_UserId1",
                table: "BookUser");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BookUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BookUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_UserId",
                table: "BookUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookUser_AspNetUsers_UserId",
                table: "BookUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookUser_AspNetUsers_UserId",
                table: "BookUser");

            migrationBuilder.DropIndex(
                name: "IX_BookUser_UserId",
                table: "BookUser");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BookUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "BookUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_UserId1",
                table: "BookUser",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookUser_AspNetUsers_UserId1",
                table: "BookUser",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
