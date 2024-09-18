using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Database.MenuContext.Migrations
{
    /// <inheritdoc />
    public partial class Menurestaurantrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                schema: "menu",
                table: "Menus",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Restaurants",
                schema: "menu",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RestaurantID", x => x.Value);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RestaurantID",
                schema: "menu",
                table: "Menus",
                column: "RestaurantID");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantId",
                schema: "menu",
                table: "Menus",
                column: "RestaurantID",
                principalSchema: "menu",
                principalTable: "Restaurants",
                principalColumn: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantId",
                schema: "menu",
                table: "Menus");

            migrationBuilder.DropTable(
                name: "Restaurants",
                schema: "menu");

            migrationBuilder.DropIndex(
                name: "IX_Menus_RestaurantID",
                schema: "menu",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                schema: "menu",
                table: "Menus");
        }
    }
}
