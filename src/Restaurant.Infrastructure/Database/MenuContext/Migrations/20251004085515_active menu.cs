using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Database.MenuContext.Migrations
{
    /// <inheritdoc />
    public partial class activemenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "menu",
                table: "Menus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Id",
                schema: "menu",
                table: "Menus",
                column: "Id",
                unique: true,
                filter: "\"IsActive\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Menus_Id",
                schema: "menu",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "menu",
                table: "Menus");
        }
    }
}
