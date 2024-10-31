using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class failurereasonremoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "events",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "events",
                table: "Menus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "events",
                table: "Restaurants",
                type: "character varying(1200)",
                maxLength: 1200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "events",
                table: "Menus",
                type: "character varying(1200)",
                maxLength: 1200,
                nullable: true);
        }
    }
}
