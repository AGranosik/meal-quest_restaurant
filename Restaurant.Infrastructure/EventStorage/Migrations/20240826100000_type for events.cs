using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.EventStorage.Migrations
{
    /// <inheritdoc />
    public partial class Typeforevents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                schema: "events",
                table: "RestaurantEvents");

            migrationBuilder.DropColumn(
                name: "Data",
                schema: "events",
                table: "MenuEvents");

            migrationBuilder.AddColumn<string>(
                name: "AssemblyName",
                schema: "events",
                table: "RestaurantEvents",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerializedData",
                schema: "events",
                table: "RestaurantEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AssemblyName",
                schema: "events",
                table: "MenuEvents",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerializedData",
                schema: "events",
                table: "MenuEvents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssemblyName",
                schema: "events",
                table: "RestaurantEvents");

            migrationBuilder.DropColumn(
                name: "SerializedData",
                schema: "events",
                table: "RestaurantEvents");

            migrationBuilder.DropColumn(
                name: "AssemblyName",
                schema: "events",
                table: "MenuEvents");

            migrationBuilder.DropColumn(
                name: "SerializedData",
                schema: "events",
                table: "MenuEvents");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                schema: "events",
                table: "RestaurantEvents",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                schema: "events",
                table: "MenuEvents",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }
    }
}
