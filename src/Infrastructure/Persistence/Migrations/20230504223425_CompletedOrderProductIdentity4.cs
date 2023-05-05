﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WOF.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompletedOrderProductIdentity4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompletedOrderProducts_CompletedOrderId",
                table: "CompletedOrderProducts");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CompletedOrderProducts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CompletedOrderProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompletedOrderProducts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CompletedOrderProducts");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CompletedOrderProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts",
                columns: new[] { "CompletedOrderId", "ProductId" });

            migrationBuilder.CreateTable(
                name: "CompletedOrderProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletedOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedOrderProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedOrderProduct_CompletedOrders_CompletedOrderId",
                        column: x => x.CompletedOrderId,
                        principalTable: "CompletedOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedOrderProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrderProducts_ProductId",
                table: "CompletedOrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrderProduct_CompletedOrderId",
                table: "CompletedOrderProduct",
                column: "CompletedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrderProduct_ProductId",
                table: "CompletedOrderProduct",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedOrderProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_CompletedOrderProducts_ProductId",
                table: "CompletedOrderProducts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CompletedOrderProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CompletedOrderProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompletedOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CompletedOrderProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CompletedOrderProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompletedOrderProducts",
                table: "CompletedOrderProducts",
                columns: new[] { "ProductId", "CompletedOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrderProducts_CompletedOrderId",
                table: "CompletedOrderProducts",
                column: "CompletedOrderId");
        }
    }
}
