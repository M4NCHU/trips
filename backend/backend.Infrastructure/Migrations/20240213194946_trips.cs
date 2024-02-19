using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class trips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Duration",
                table: "VisitPlace",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "VisitPlace",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "VisitPlace",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DayNumber",
                table: "TripDestination",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Trip",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Trip",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "VisitPlace");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "VisitPlace");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "VisitPlace");

            migrationBuilder.DropColumn(
                name: "DayNumber",
                table: "TripDestination");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Trip");
        }
    }
}
