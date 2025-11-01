using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Database.MenuContext.Migrations
{
    /// <inheritdoc />
    public partial class categoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Value",
                schema: "menu",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "menu",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                schema: "menu",
                table: "Categories",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "menu",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                schema: "menu",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                schema: "menu",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "menu",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "menu",
                table: "Categories",
                newName: "CategoryID");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                schema: "menu",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Value",
                schema: "menu",
                table: "Categories",
                column: "Value",
                unique: true);
        }
    }
}
