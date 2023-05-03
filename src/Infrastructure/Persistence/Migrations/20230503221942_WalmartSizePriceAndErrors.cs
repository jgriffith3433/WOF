using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WalmartSizePriceAndErrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalmartItemResponse",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalmartLink",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalmartSize",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WalmartItemResponse",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WalmartLink",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WalmartSize",
                table: "Products");
        }
    }
}
