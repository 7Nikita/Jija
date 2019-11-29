using Microsoft.EntityFrameworkCore.Migrations;

namespace Jija.Migrations
{
    public partial class UpdatedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GithubUsers_UserId",
                table: "GithubUsers");

            migrationBuilder.CreateIndex(
                name: "IX_GithubUsers_UserId",
                table: "GithubUsers",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GithubUsers_UserId",
                table: "GithubUsers");

            migrationBuilder.CreateIndex(
                name: "IX_GithubUsers_UserId",
                table: "GithubUsers",
                column: "UserId");
        }
    }
}
