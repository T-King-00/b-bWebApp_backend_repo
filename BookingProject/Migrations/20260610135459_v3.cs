using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingProject.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bed",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Price",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "RoomType",
                table: "Rooms",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Rooms",
                newName: "RoomType");

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "City", "Country", "CreationDate", "Description", "Name" },
                values: new object[] { 1, "Skagen", "Denmark", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Danish hotel owned by Pernilles Bed and Breakfast", "Danish Bed and Breakfast" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "HotelId", "RoomType", "size" },
                values: new object[] { 1, 1, 0, 10 });

            migrationBuilder.InsertData(
                table: "Bed",
                columns: new[] { "Id", "Quantity", "RoomId", "Type" },
                values: new object[] { 1, 1, 1, 0 });

            migrationBuilder.InsertData(
                table: "Price",
                columns: new[] { "Id", "BasePrice", "RoomId" },
                values: new object[] { 1, 100.0, 1 });
        }
    }
}
