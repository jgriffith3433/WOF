using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NullableProductStockColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalledIngredients_ProductStocks_ProductStockId",
                table: "CalledIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "ProductStockId",
                table: "CalledIngredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CalledIngredients_ProductStocks_ProductStockId",
                table: "CalledIngredients",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalledIngredients_ProductStocks_ProductStockId",
                table: "CalledIngredients");

            migrationBuilder.AlterColumn<int>(
                name: "ProductStockId",
                table: "CalledIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CalledIngredients_ProductStocks_ProductStockId",
                table: "CalledIngredients",
                column: "ProductStockId",
                principalTable: "ProductStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
