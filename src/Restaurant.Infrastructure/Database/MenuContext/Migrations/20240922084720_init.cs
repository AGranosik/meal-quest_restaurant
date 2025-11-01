using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Database.MenuContext.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "menu");

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
                name: "Restaurants",
                schema: "menu",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RestaurantID", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "GroupMeals",
                schema: "menu",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "integer", nullable: false),
                    MealID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMeals", x => new { x.GroupID, x.MealID });
                    table.ForeignKey(
                        name: "FK_GroupMeals_Groups_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "menu",
                        principalTable: "Groups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMeals_Meals_MealID",
                        column: x => x.MealID,
                        principalSchema: "menu",
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealIngredients",
                schema: "menu",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "integer", nullable: false),
                    MealID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealIngredients", x => new { x.GroupID, x.MealID });
                    table.ForeignKey(
                        name: "FK_MealIngredients_Ingredients_MealID",
                        column: x => x.MealID,
                        principalSchema: "menu",
                        principalTable: "Ingredients",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealIngredients_Meals_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "menu",
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RestaurantID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantId",
                        column: x => x.RestaurantID,
                        principalSchema: "menu",
                        principalTable: "Restaurants",
                        principalColumn: "Value");
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

            migrationBuilder.CreateIndex(
                name: "IX_GroupMeals_MealID",
                schema: "menu",
                table: "GroupMeals",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMenu_MenuId",
                schema: "menu",
                table: "GroupMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MealIngredients_MealID",
                schema: "menu",
                table: "MealIngredients",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RestaurantID",
                schema: "menu",
                table: "Menus",
                column: "RestaurantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMeals",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "GroupMenu",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "MealIngredients",
                schema: "menu");

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
                name: "Restaurants",
                schema: "menu");
        }
    }
}
