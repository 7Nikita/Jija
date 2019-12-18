using Microsoft.EntityFrameworkCore.Migrations;

namespace Jija.Migrations
{
    public partial class AddedAsgModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignees_AspNetUsers_AssignedUserId",
                table: "Assignees");

            migrationBuilder.DropIndex(
                name: "IX_Assignees_AssignedUserId",
                table: "Assignees");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Assignees");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignees_AspNetUsers_AssigneeId",
                table: "Assignees",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignees_AspNetUsers_AssigneeId",
                table: "Assignees");

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Assignees",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignees_AssignedUserId",
                table: "Assignees",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignees_AspNetUsers_AssignedUserId",
                table: "Assignees",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
