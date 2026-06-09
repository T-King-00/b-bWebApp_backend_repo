using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingProject.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "Bed");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Bed",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Bed",
                keyColumn: "Id",
                keyValue: 1,
                column: "Available",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");
        }
    }
}
