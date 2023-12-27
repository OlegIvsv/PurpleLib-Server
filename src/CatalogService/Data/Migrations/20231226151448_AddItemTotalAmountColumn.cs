using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTotalAmountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "CatalogItems");

            migrationBuilder.AddColumn<Guid>(
                name: "SellerId",
                table: "CatalogItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "CatalogItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CatalogItems");

            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "CatalogItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
