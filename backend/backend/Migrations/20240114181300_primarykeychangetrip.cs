using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    public partial class primarykeychangetrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedPlace_TripDestinations_TripDestinationTripId_TripDe~",
                table: "SelectedPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripDestinations",
                table: "TripDestinations");

            migrationBuilder.DropIndex(
                name: "IX_SelectedPlace_TripDestinationTripId_TripDestinationDestinat~",
                table: "SelectedPlace");

            migrationBuilder.DropColumn(
                name: "TripDestinationDestinationId",
                table: "SelectedPlace");

            migrationBuilder.DropColumn(
                name: "TripDestinationTripId",
                table: "SelectedPlace");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TripDestinations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripDestinations",
                table: "TripDestinations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TripDestinations_TripId",
                table: "TripDestinations",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedPlace_TripDestinationId",
                table: "SelectedPlace",
                column: "TripDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedPlace_TripDestinations_TripDestinationId",
                table: "SelectedPlace",
                column: "TripDestinationId",
                principalTable: "TripDestinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedPlace_TripDestinations_TripDestinationId",
                table: "SelectedPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripDestinations",
                table: "TripDestinations");

            migrationBuilder.DropIndex(
                name: "IX_TripDestinations_TripId",
                table: "TripDestinations");

            migrationBuilder.DropIndex(
                name: "IX_SelectedPlace_TripDestinationId",
                table: "SelectedPlace");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TripDestinations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "TripDestinationDestinationId",
                table: "SelectedPlace",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TripDestinationTripId",
                table: "SelectedPlace",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripDestinations",
                table: "TripDestinations",
                columns: new[] { "TripId", "DestinationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedPlace_TripDestinationTripId_TripDestinationDestinat~",
                table: "SelectedPlace",
                columns: new[] { "TripDestinationTripId", "TripDestinationDestinationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedPlace_TripDestinations_TripDestinationTripId_TripDe~",
                table: "SelectedPlace",
                columns: new[] { "TripDestinationTripId", "TripDestinationDestinationId" },
                principalTable: "TripDestinations",
                principalColumns: new[] { "TripId", "DestinationId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
