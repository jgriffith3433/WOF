using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCookConsumeRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumedCookedRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Servings = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedCookedRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumedCookedRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CookedRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookedRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CookedRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CookedRecipeCalledIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CookedRecipeId = table.Column<int>(type: "int", nullable: false),
                    CalledIngredientId = table.Column<int>(type: "int", nullable: true),
                    ProductStockId = table.Column<int>(type: "int", nullable: false),
                    SizeType = table.Column<int>(type: "int", nullable: false),
                    Units = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookedRecipeCalledIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CookedRecipeCalledIngredients_CalledIngredients_CalledIngredientId",
                        column: x => x.CalledIngredientId,
                        principalTable: "CalledIngredients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CookedRecipeCalledIngredients_CookedRecipes_CookedRecipeId",
                        column: x => x.CookedRecipeId,
                        principalTable: "CookedRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CookedRecipeCalledIngredients_ProductStocks_ProductStockId",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedCookedRecipes_RecipeId",
                table: "ConsumedCookedRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_CookedRecipeCalledIngredients_CalledIngredientId",
                table: "CookedRecipeCalledIngredients",
                column: "CalledIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_CookedRecipeCalledIngredients_CookedRecipeId",
                table: "CookedRecipeCalledIngredients",
                column: "CookedRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_CookedRecipeCalledIngredients_ProductStockId",
                table: "CookedRecipeCalledIngredients",
                column: "ProductStockId");

            migrationBuilder.CreateIndex(
                name: "IX_CookedRecipes_RecipeId",
                table: "CookedRecipes",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumedCookedRecipes");

            migrationBuilder.DropTable(
                name: "CookedRecipeCalledIngredients");

            migrationBuilder.DropTable(
                name: "CookedRecipes");
        }
    }
}
