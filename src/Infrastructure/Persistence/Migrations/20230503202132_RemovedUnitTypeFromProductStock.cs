using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnitTypeFromProductStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalledIngredients_Products_ProductId",
                table: "CalledIngredients");

            migrationBuilder.DropIndex(
                name: "IX_CalledIngredients_ProductId",
                table: "CalledIngredients");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "ProductStocks");

            migrationBuilder.AlterColumn<float>(
                name: "Units",
                table: "ProductStocks",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<float>(
                name: "Units",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ProductStockId",
                table: "CalledIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CalledIngredients_ProductStockId",
                table: "CalledIngredients",
                column: "ProductStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalledIngredients_ProductStocks_ProductStockId",
                table: "CalledIngredients",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalledIngredients_ProductStocks_ProductStockId",
                table: "CalledIngredients");

            migrationBuilder.DropIndex(
                name: "IX_CalledIngredients_ProductStockId",
                table: "CalledIngredients");

            migrationBuilder.DropColumn(
                name: "Units",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductStockId",
                table: "CalledIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "Units",
                table: "ProductStocks",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "ProductStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CalledIngredients_ProductId",
                table: "CalledIngredients",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalledIngredients_Products_ProductId",
                table: "CalledIngredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
