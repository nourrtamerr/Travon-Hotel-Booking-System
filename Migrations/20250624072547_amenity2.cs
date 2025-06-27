using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCBookingFinal_YARAB_.Migrations
{
    /// <inheritdoc />
    public partial class amenity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "effed422-d3fe-4462-a884-304f87da9eeb");

            migrationBuilder.UpdateData(
                table: "DraftReservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate" },
                values: new object[] { new DateTime(2025, 6, 24, 10, 25, 46, 725, DateTimeKind.Local).AddTicks(3038), new DateTime(2025, 6, 29, 10, 25, 46, 725, DateTimeKind.Local).AddTicks(3074) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 6, 24, 10, 25, 46, 726, DateTimeKind.Local).AddTicks(9050));

            migrationBuilder.UpdateData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpiryDate",
                value: new DateTime(2025, 6, 29, 10, 25, 46, 727, DateTimeKind.Local).AddTicks(3085));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 24, 10, 25, 46, 727, DateTimeKind.Local).AddTicks(7015));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate" },
                values: new object[] { new DateTime(2025, 6, 24, 10, 25, 46, 728, DateTimeKind.Local).AddTicks(1162), new DateTime(2025, 6, 29, 10, 25, 46, 728, DateTimeKind.Local).AddTicks(1205) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 24, 10, 25, 46, 728, DateTimeKind.Local).AddTicks(7140));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "fc281ddb-af57-4b56-9d5d-1a6e544e7d9c");

            migrationBuilder.UpdateData(
                table: "DraftReservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate" },
                values: new object[] { new DateTime(2025, 6, 24, 10, 20, 53, 548, DateTimeKind.Local).AddTicks(6077), new DateTime(2025, 6, 29, 10, 20, 53, 548, DateTimeKind.Local).AddTicks(6116) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 6, 24, 10, 20, 53, 549, DateTimeKind.Local).AddTicks(3772));

            migrationBuilder.UpdateData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpiryDate",
                value: new DateTime(2025, 6, 29, 10, 20, 53, 549, DateTimeKind.Local).AddTicks(5476));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 24, 10, 20, 53, 549, DateTimeKind.Local).AddTicks(7157));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate" },
                values: new object[] { new DateTime(2025, 6, 24, 10, 20, 53, 549, DateTimeKind.Local).AddTicks(8887), new DateTime(2025, 6, 29, 10, 20, 53, 549, DateTimeKind.Local).AddTicks(8901) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 6, 24, 10, 20, 53, 550, DateTimeKind.Local).AddTicks(2326));
        }
    }
}
