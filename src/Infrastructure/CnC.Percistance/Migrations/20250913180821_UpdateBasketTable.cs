using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnC.Percistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBasketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductBaskets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductBaskets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductBaskets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductBaskets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductBaskets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductBaskets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductBaskets");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductBaskets");
        }
    }
}
