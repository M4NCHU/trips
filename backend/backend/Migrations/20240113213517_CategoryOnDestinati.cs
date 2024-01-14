using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class CategoryOnDestinati : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Destinations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_CategoryId",
                table: "Destinations",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Category_CategoryId",
                table: "Destinations",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Category_CategoryId",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_CategoryId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Destinations");
        }
    }
}
