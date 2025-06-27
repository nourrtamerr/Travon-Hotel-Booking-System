using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCBookingFinal_YARAB_.Migrations
{
    /// <inheritdoc />
    public partial class amenity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEmail",
                table: "RegisterViewModel");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmPassword",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Usernameoremail",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "loginPassword",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "rememberme",
                table: "RegisterViewModel",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReviewViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewViewModel", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amenities",
                value: 196);

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
                table: "PaymentMethod",
                keyColumn: "Id",
                keyValue: 25,
                column: "ExpiryDate",
                value: new DateOnly(2028, 6, 24));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewViewModel");

            migrationBuilder.DropColumn(
                name: "Usernameoremail",
                table: "RegisterViewModel");

            migrationBuilder.DropColumn(
                name: "loginPassword",
                table: "RegisterViewModel");

            migrationBuilder.DropColumn(
                name: "rememberme",
                table: "RegisterViewModel");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmPassword",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmEmail",
                table: "RegisterViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amenities",
                value: 98);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7620c9eb-158f-4338-a8af-e6c52626f403");

            migrationBuilder.UpdateData(
                table: "DraftReservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate" },
                values: new object[] { new DateTime(2025, 3, 8, 2, 28, 31, 443, DateTimeKind.Local).AddTicks(2125), new DateTime(2025, 3, 13, 2, 28, 31, 443, DateTimeKind.Local).AddTicks(2133) });

            migrationBuilder.UpdateData(
                table: "PaymentMethod",
                keyColumn: "Id",
                keyValue: 25,
                column: "ExpiryDate",
                value: new DateOnly(2028, 3, 8));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentDate",
                value: new DateTime(2025, 3, 8, 2, 28, 31, 444, DateTimeKind.Local).AddTicks(861));

            migrationBuilder.UpdateData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpiryDate",
                value: new DateTime(2025, 3, 13, 2, 28, 31, 444, DateTimeKind.Local).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 3, 8, 2, 28, 31, 444, DateTimeKind.Local).AddTicks(4338));

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate" },
                values: new object[] { new DateTime(2025, 3, 8, 2, 28, 31, 444, DateTimeKind.Local).AddTicks(6213), new DateTime(2025, 3, 13, 2, 28, 31, 444, DateTimeKind.Local).AddTicks(6217) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 3, 8, 2, 28, 31, 444, DateTimeKind.Local).AddTicks(9776));
        }
    }
}
