using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnC.Percistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSomeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductBaskets");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderProducts");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "PaymentMethods",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "PaymentMethods");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductBaskets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
