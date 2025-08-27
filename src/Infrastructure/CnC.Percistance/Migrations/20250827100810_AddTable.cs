using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnC.Percistance.Migrations
{
    /// <inheritdoc />
    public partial class AddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ProductCurrencies");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProductCurrencies",
                newName: "ConvertedPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAzn",
                table: "Products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyRateId",
                table: "ProductCurrencies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    RateAgainstAzn = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCurrencies_CurrencyRateId",
                table: "ProductCurrencies",
                column: "CurrencyRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCurrencies_CurrencyRates_CurrencyRateId",
                table: "ProductCurrencies",
                column: "CurrencyRateId",
                principalTable: "CurrencyRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCurrencies_CurrencyRates_CurrencyRateId",
                table: "ProductCurrencies");

            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropIndex(
                name: "IX_ProductCurrencies_CurrencyRateId",
                table: "ProductCurrencies");

            migrationBuilder.DropColumn(
                name: "PriceAzn",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CurrencyRateId",
                table: "ProductCurrencies");

            migrationBuilder.RenameColumn(
                name: "ConvertedPrice",
                table: "ProductCurrencies",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "ProductCurrencies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
