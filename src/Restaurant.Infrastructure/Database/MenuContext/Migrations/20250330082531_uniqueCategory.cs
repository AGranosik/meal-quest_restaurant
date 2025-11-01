using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Database.MenuContext.Migrations
{
    /// <inheritdoc />
    public partial class uniqueCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantId",
                schema: "menu",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_RestaurantID",
                schema: "menu",
                table: "Menus");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                schema: "menu",
                table: "Menus",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Value",
                schema: "menu",
                table: "Categories",
                column: "Value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Value",
                schema: "menu",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                schema: "menu",
                table: "Menus",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
                principalColumn: "Id");
        }
    }
}
