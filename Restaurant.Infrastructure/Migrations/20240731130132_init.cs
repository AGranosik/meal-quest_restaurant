using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "restaurant");

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "restaurant",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Street = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Coordinates_X = table.Column<double>(type: "double precision", nullable: false),
                    Coordinates_Y = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressID);
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                schema: "restaurant",
                columns: table => new
                {
                    OpeningHoursID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.OpeningHoursID);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                schema: "restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    AddressID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Owners_Addresses_AddressID",
                        column: x => x.AddressID,
                        principalSchema: "restaurant",
                        principalTable: "Addresses",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDays",
                schema: "restaurant",
                columns: table => new
                {
                    WorkingDayID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    From = table.Column<TimeSpan>(type: "interval", nullable: false),
                    To = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Free = table.Column<bool>(type: "boolean", nullable: false),
                    OpeningHoursID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDays", x => x.WorkingDayID);
                    table.ForeignKey(
                        name: "FK_WorkingDays_OpeningHours_OpeningHoursID",
                        column: x => x.OpeningHoursID,
                        principalSchema: "restaurant",
                        principalTable: "OpeningHours",
                        principalColumn: "OpeningHoursID");
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                schema: "restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    OpeningHoursID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_OpeningHours_OpeningHoursID",
                        column: x => x.OpeningHoursID,
                        principalSchema: "restaurant",
                        principalTable: "OpeningHours",
                        principalColumn: "OpeningHoursID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurants_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "restaurant",
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Owners_AddressID",
                schema: "restaurant",
                table: "Owners",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OpeningHoursID",
                schema: "restaurant",
                table: "Restaurants",
                column: "OpeningHoursID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                schema: "restaurant",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_OpeningHoursID",
                schema: "restaurant",
                table: "WorkingDays",
                column: "OpeningHoursID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restaurants",
                schema: "restaurant");

            migrationBuilder.DropTable(
                name: "WorkingDays",
                schema: "restaurant");

            migrationBuilder.DropTable(
                name: "Owners",
                schema: "restaurant");

            migrationBuilder.DropTable(
                name: "OpeningHours",
                schema: "restaurant");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "restaurant");
        }
    }
}
