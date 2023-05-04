using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCalledIngredientToForignKeyRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalledIngredientRecipes");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "CalledIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeType",
                table: "CalledIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Units",
                table: "CalledIngredients",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_CalledIngredients_RecipeId",
                table: "CalledIngredients",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalledIngredients_Recipes_RecipeId",
                table: "CalledIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalledIngredients_Recipes_RecipeId",
                table: "CalledIngredients");

            migrationBuilder.DropIndex(
                name: "IX_CalledIngredients_RecipeId",
                table: "CalledIngredients");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "CalledIngredients");

            migrationBuilder.DropColumn(
                name: "SizeType",
                table: "CalledIngredients");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "CalledIngredients");

            migrationBuilder.CreateTable(
                name: "CalledIngredientRecipes",
                columns: table => new
                {
                    CalledIngredientId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalledIngredientRecipes", x => new { x.CalledIngredientId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_CalledIngredientRecipes_CalledIngredients_CalledIngredientId",
                        column: x => x.CalledIngredientId,
                        principalTable: "CalledIngredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalledIngredientRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalledIngredientRecipes_RecipeId",
                table: "CalledIngredientRecipes",
                column: "RecipeId");
        }
    }
}
