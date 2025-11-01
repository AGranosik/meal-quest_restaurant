using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Database.RestaurantContext.Migrations
{
    /// <inheritdoc />
    public partial class restaurantDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "restaurant",
                table: "Restaurants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "restaurant",
                table: "Restaurants");
        }
    }
}
