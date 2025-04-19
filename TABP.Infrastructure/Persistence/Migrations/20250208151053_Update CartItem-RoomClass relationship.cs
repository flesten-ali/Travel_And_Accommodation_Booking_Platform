using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItemRoomClassrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemRoomClass");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomClassId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_RoomClassId",
                table: "CartItems",
                column: "RoomClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_RoomClasses_RoomClassId",
                table: "CartItems",
                column: "RoomClassId",
                principalTable: "RoomClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_RoomClasses_RoomClassId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_RoomClassId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "RoomClassId",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "CartItemRoomClass",
                columns: table => new
                {
                    CartItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomClassesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemRoomClass", x => new { x.CartItemsId, x.RoomClassesId });
                    table.ForeignKey(
                        name: "FK_CartItemRoomClass_CartItems_CartItemsId",
                        column: x => x.CartItemsId,
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemRoomClass_RoomClasses_RoomClassesId",
                        column: x => x.RoomClassesId,
                        principalTable: "RoomClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemRoomClass_RoomClassesId",
                table: "CartItemRoomClass",
                column: "RoomClassesId");
        }
    }
}
