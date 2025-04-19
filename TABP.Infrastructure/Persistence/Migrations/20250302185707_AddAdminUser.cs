using Microsoft.EntityFrameworkCore.Migrations;
using TABP.Domain.Constants;

#nullable disable

namespace TABP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "Users",
                 columns: ["Id", "UserName", "Email", "PasswordHash", "Role"],
                 values:
                 [
                Guid.NewGuid(),
                "admin",
                "admin@example.com",
                "AQAAAAIAAYagAAAAEKEkygIjOMgtLQ8PuQx8r+UZigqwullsU7s1UZEIpjya4FGkSO0foKKlxC2VhTmkmA==",
                Roles.Admin
                 ]
             );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "UserName",
            keyValue: "admin"
            );
        }
    }
}
