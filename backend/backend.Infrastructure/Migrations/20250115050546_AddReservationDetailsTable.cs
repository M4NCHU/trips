using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "ReservationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AdditionalNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationDetails_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DestinationDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GeoLocationId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    TripId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    DayNumber = table.Column<int>(type: "integer", nullable: false)
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
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationDTOId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    TripDestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitPlaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TripDestinationDTOId = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "IX_ReservationDetails_ReservationId",
                table: "ReservationDetails",
                column: "ReservationId",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_DestinationDTO_DestinationId",
                table: "Cart");

            migrationBuilder.DropTable(
                name: "ReservationDetails");

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
    }
}
