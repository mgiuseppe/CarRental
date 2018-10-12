using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalNovility.DataLayer.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CarTypes",
                columns: new[] { "Id", "CancellationRateFee", "DepositFee", "Name", "RentalRateFee" },
                values: new object[,]
                {
                    { 1, 5m, 0.2m, "Family Car", 10m },
                    { 2, 50m, 0.2m, "Sport Car", 100m }
                });

            migrationBuilder.InsertData(
                table: "ClientAccount",
                columns: new[] { "Id", "CancellationFeePaid", "CancellationFeeValueAtBookingMoment", "DepositFeePaid", "RentalFeePaid", "RentalRateFeeValueAtBookingMoment" },
                values: new object[] { 1L, 0m, 5m, 2.0m, 0m, 10m });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Email", "FullName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1L, "giuseppem@test.com", "Giuseppe M", "3476010101" },
                    { 2L, "barban@test.com", "Barba N", "3476010202" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "PlateNumber", "TypeId" },
                values: new object[] { 1L, "AA00BB", 1 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "PlateNumber", "TypeId" },
                values: new object[] { 2L, "AA01BB", 1 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "PlateNumber", "TypeId" },
                values: new object[] { 3L, "AA00SP", 2 });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CarId", "ClientAccountId", "ClientId", "PickUpDateTime", "ReturnDateTime", "State" },
                values: new object[] { 1L, 1L, 1L, 1L, new DateTime(2018, 10, 10, 2, 30, 35, 737, DateTimeKind.Local), new DateTime(2018, 10, 10, 10, 30, 35, 739, DateTimeKind.Local), 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "CarTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ClientAccount",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "CarTypes",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
