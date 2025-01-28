using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationDetailsTableupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_DestinationDTO_DestinationId",
                table: "Cart");

            migrationBuilder.DropTable(
                name: "SelectedPlaceDTO");

            migrationBuilder.DropTable(
                name: "TripDestinationDTO");

            migrationBuilder.DropTable(
                name: "VisitPlaceDTO");

            migrationBuilder.DropTable(
                name: "DestinationDTO");

            migrationBuilder.DropTable(
                name: "CategoryDTO");

            migrationBuilder.DropTable(
                name: "GeoLocationDTO");

            migrationBuilder.DropIndex(
                name: "IX_Cart_DestinationId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "Cart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DestinationId",
                table: "Cart",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDTO", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "DestinationDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    GeoLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DestinationDTO_CategoryDTO_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryDTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DestinationDTO_GeoLocationDTO_GeoLocationId",
                        column: x => x.GeoLocationId,
                        principalTable: "GeoLocationDTO",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TripDestinationDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TripId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripDestinationDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripDestinationDTO_DestinationDTO_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "DestinationDTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitPlaceDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DestinationDTOId = table.Column<Guid>(type: "uuid", nullable: true),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitPlaceDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitPlaceDTO_DestinationDTO_DestinationDTOId",
                        column: x => x.DestinationDTOId,
                        principalTable: "DestinationDTO",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SelectedPlaceDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitPlaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TripDestinationDTOId = table.Column<Guid>(type: "uuid", nullable: true),
                    TripDestinationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedPlaceDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedPlaceDTO_TripDestinationDTO_TripDestinationDTOId",
                        column: x => x.TripDestinationDTOId,
                        principalTable: "TripDestinationDTO",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SelectedPlaceDTO_VisitPlaceDTO_VisitPlaceId",
                        column: x => x.VisitPlaceId,
                        principalTable: "VisitPlaceDTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_DestinationId",
                table: "Cart",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_DestinationDTO_CategoryId",
                table: "DestinationDTO",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DestinationDTO_GeoLocationId",
                table: "DestinationDTO",
                column: "GeoLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedPlaceDTO_TripDestinationDTOId",
                table: "SelectedPlaceDTO",
                column: "TripDestinationDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedPlaceDTO_VisitPlaceId",
                table: "SelectedPlaceDTO",
                column: "VisitPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_TripDestinationDTO_DestinationId",
                table: "TripDestinationDTO",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitPlaceDTO_DestinationDTOId",
                table: "VisitPlaceDTO",
                column: "DestinationDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_DestinationDTO_DestinationId",
                table: "Cart",
                column: "DestinationId",
                principalTable: "DestinationDTO",
                principalColumn: "Id");
        }
    }
}
