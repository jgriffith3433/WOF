using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompletedOrderProductIdentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompletedOrderProducts_ProductId",
                table: "CompletedOrderProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts",
                columns: new[] { "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrderProducts_ProductId",
                table: "CompletedOrderProducts",
                column: "ProductId");
        }
    }
}
