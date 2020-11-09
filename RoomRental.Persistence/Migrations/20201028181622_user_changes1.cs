using Microsoft.EntityFrameworkCore.Migrations;

namespace RoomRental.Persistence.Migrations
{
    public partial class user_changes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LandlordUserId",
                table: "Reservations",
                column: "LandlordUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TenantUserId",
                table: "Reservations",
                column: "TenantUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_LandlordUserId",
                table: "Reservations",
                column: "LandlordUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_TenantUserId",
                table: "Reservations",
                column: "TenantUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_LandlordUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_TenantUserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_LandlordUserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TenantUserId",
                table: "Reservations");
        }
    }
}
