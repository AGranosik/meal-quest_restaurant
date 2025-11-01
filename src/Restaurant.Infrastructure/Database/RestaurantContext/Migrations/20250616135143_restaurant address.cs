using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Database.RestaurantContext.Migrations
{
    /// <inheritdoc />
    public partial class restaurantaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                schema: "restaurant",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_AddressID",
                schema: "restaurant",
                table: "Restaurants",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Addresses_AddressID",
                schema: "restaurant",
                table: "Restaurants",
                column: "AddressID",
                principalSchema: "restaurant",
                principalTable: "Addresses",
                principalColumn: "AddressID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Addresses_AddressID",
                schema: "restaurant",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_AddressID",
                schema: "restaurant",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "AddressID",
                schema: "restaurant",
                table: "Restaurants");
        }
    }
}
