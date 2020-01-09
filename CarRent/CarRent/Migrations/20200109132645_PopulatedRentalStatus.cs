using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRent.Migrations
{
    public partial class PopulatedRentalStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Rentals",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Rentals",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RentDate",
                table: "Rentals",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "RentalStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { (byte)1, "Reservation" });

            migrationBuilder.InsertData(
                table: "RentalStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { (byte)2, "Checked" });

            migrationBuilder.InsertData(
                table: "RentalStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { (byte)3, "Archival" });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_AspNetUsers_UserId",
                table: "Rentals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_AspNetUsers_UserId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals");

            migrationBuilder.DeleteData(
                table: "RentalStatus",
                keyColumn: "Id",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                table: "RentalStatus",
                keyColumn: "Id",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                table: "RentalStatus",
                keyColumn: "Id",
                keyValue: (byte)3);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RentDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
