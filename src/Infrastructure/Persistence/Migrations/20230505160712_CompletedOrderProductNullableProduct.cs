using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompletedOrderProductNullableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedOrderProducts_Products_ProductId",
                table: "CompletedOrderProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "CompletedOrderProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedOrderProducts_Products_ProductId",
                table: "CompletedOrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedOrderProducts_Products_ProductId",
                table: "CompletedOrderProducts");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "CompletedOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedOrderProducts_Products_ProductId",
                table: "CompletedOrderProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
