using Microsoft.EntityFrameworkCore.Migrations;

namespace RoomRental.Persistence.Migrations
{
    public partial class user_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LandlordUserId",
                table: "Rooms",
                column: "LandlordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Users_LandlordUserId",
                table: "Rooms",
                column: "LandlordUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Users_LandlordUserId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LandlordUserId",
                table: "Rooms");
        }
    }
}
