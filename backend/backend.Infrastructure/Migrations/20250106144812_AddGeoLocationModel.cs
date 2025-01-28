using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeoLocationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeoLocationId",
                table: "Destination",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeoLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoLocation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destination_GeoLocationId",
                table: "Destination",
                column: "GeoLocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Destination_GeoLocation_GeoLocationId",
                table: "Destination",
                column: "GeoLocationId",
                principalTable: "GeoLocation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destination_GeoLocation_GeoLocationId",
                table: "Destination");

            migrationBuilder.DropTable(
                name: "GeoLocation");

            migrationBuilder.DropIndex(
                name: "IX_Destination_GeoLocationId",
                table: "Destination");

            migrationBuilder.DropColumn(
                name: "GeoLocationId",
                table: "Destination");
        }
    }
}
