using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Data.Migrations
{
    public partial class RemovePrivateMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_FriendId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FriendId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FriendId",
                table: "Messages",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_FriendId",
                table: "Messages",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
