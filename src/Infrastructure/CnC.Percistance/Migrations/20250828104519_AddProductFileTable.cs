using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnC.Percistance.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Downloads_OrderProductId",
                table: "Downloads");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Downloads",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductFilesId",
                table: "Downloads",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProductFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ProductDescriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFiles_ProductDescriptions_ProductDescriptionId",
                        column: x => x.ProductDescriptionId,
                        principalTable: "ProductDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_OrderProductId",
                table: "Downloads",
                column: "OrderProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_ProductFilesId",
                table: "Downloads",
                column: "ProductFilesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFiles_ProductDescriptionId",
                table: "ProductFiles",
                column: "ProductDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Downloads_ProductFiles_ProductFilesId",
                table: "Downloads",
                column: "ProductFilesId",
                principalTable: "ProductFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Downloads_ProductFiles_ProductFilesId",
                table: "Downloads");

            migrationBuilder.DropTable(
                name: "ProductFiles");

            migrationBuilder.DropIndex(
                name: "IX_Downloads_OrderProductId",
                table: "Downloads");

            migrationBuilder.DropIndex(
                name: "IX_Downloads_ProductFilesId",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "ProductFilesId",
                table: "Downloads");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Downloads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateIndex(
                name: "IX_Downloads_OrderProductId",
                table: "Downloads",
                column: "OrderProductId",
                unique: true);
        }
    }
}
