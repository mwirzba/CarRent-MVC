using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRent.Migrations
{
    public partial class Added_Rental_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RentalStatusId",
                table: "Rentals",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "RentalStatus",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_RentalStatusId",
                table: "Rentals",
                column: "RentalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_RentalStatus_RentalStatusId",
                table: "Rentals",
                column: "RentalStatusId",
                principalTable: "RentalStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_RentalStatus_RentalStatusId",
                table: "Rentals");

            migrationBuilder.DropTable(
                name: "RentalStatus");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_RentalStatusId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "RentalStatusId",
                table: "Rentals");
        }
    }
}
