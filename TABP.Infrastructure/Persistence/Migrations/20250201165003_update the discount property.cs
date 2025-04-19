using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatethediscountproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Percentage",
                table: "Discounts",
                type: "float(8)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Percentage",
                table: "Discounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(8)",
                oldPrecision: 8,
                oldScale: 2);
        }
    }
}
