using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Database.RestaurantContext.Migrations
{
    /// <inheritdoc />
    public partial class restaurantLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                schema: "restaurant",
                table: "Restaurants",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                schema: "restaurant",
                table: "Restaurants");
        }
    }
}
