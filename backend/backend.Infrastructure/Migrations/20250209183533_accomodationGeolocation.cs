using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class accomodationGeolocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeoLocationId",
                table: "Accommodation",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeoLocationDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLocationDTO", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_GeoLocationId",
                table: "Accommodation",
                column: "GeoLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodation_GeoLocationDTO_GeoLocationId",
                table: "Accommodation",
                column: "GeoLocationId",
                principalTable: "GeoLocationDTO",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodation_GeoLocationDTO_GeoLocationId",
                table: "Accommodation");

            migrationBuilder.DropTable(
                name: "GeoLocationDTO");

            migrationBuilder.DropIndex(
                name: "IX_Accommodation_GeoLocationId",
                table: "Accommodation");

            migrationBuilder.DropColumn(
                name: "GeoLocationId",
                table: "Accommodation");
        }
    }
}
