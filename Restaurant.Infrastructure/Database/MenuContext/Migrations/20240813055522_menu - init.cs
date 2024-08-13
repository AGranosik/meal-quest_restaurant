using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Database.MenuContext.Migrations
{
    /// <inheritdoc />
    public partial class menuinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "restaurant");

            migrationBuilder.EnsureSchema(
                name: "menu");

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
                name: "Groups",
                schema: "menu",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupID);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                schema: "menu",
                columns: table => new
                {
                    IngredientID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientID);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                schema: "menu",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.MealID);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "GroupMeal",
                schema: "menu",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "integer", nullable: false),
                    MealsMealID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMeal", x => new { x.GroupID, x.MealsMealID });
                    table.ForeignKey(
                        name: "FK_GroupMeal_Groups_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "menu",
                        principalTable: "Groups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMeal_Meals_MealsMealID",
                        column: x => x.MealsMealID,
                        principalSchema: "menu",
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IngredientMeal",
                schema: "menu",
                columns: table => new
                {
                    IngredientsIngredientID = table.Column<int>(type: "integer", nullable: false),
                    MealID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientMeal", x => new { x.IngredientsIngredientID, x.MealID });
                    table.ForeignKey(
                        name: "FK_IngredientMeal_Ingredients_IngredientsIngredientID",
                        column: x => x.IngredientsIngredientID,
                        principalSchema: "menu",
                        principalTable: "Ingredients",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientMeal_Meals_MealID",
                        column: x => x.MealID,
                        principalSchema: "menu",
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMenu",
                schema: "menu",
                columns: table => new
                {
                    GroupsGroupID = table.Column<int>(type: "integer", nullable: false),
                    MenuId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMenu", x => new { x.GroupsGroupID, x.MenuId });
                    table.ForeignKey(
                        name: "FK_GroupMenu_Groups_GroupsGroupID",
                        column: x => x.GroupsGroupID,
                        principalSchema: "menu",
                        principalTable: "Groups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMenu_Menus_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "menu",
                        principalTable: "Menus",
                        principalColumn: "Id",
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
                name: "IX_GroupMeal_MealsMealID",
                schema: "menu",
                table: "GroupMeal",
                column: "MealsMealID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMenu_MenuId",
                schema: "menu",
                table: "GroupMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientMeal_MealID",
                schema: "menu",
                table: "IngredientMeal",
                column: "MealID");

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
                name: "GroupMeal",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "GroupMenu",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "IngredientMeal",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "Restaurants",
                schema: "restaurant");

            migrationBuilder.DropTable(
                name: "WorkingDays",
                schema: "restaurant");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "Ingredients",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "Meals",
                schema: "menu");

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
