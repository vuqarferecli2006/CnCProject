using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnC.Percistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductViewsProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductViews_AspNetUsers_UserId",
                table: "ProductViews");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductViews",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "ProductViews",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductViews_AspNetUsers_UserId",
                table: "ProductViews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductViews_AspNetUsers_UserId",
                table: "ProductViews");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductViews",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SessionId",
                table: "ProductViews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductViews_AspNetUsers_UserId",
                table: "ProductViews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
