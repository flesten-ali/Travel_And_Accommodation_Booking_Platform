using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemRoom");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemRoomClass");

            migrationBuilder.CreateTable(
                name: "CartItemRoom",
                columns: table => new
                {
                    CartItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemRoom", x => new { x.CartItemsId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_CartItemRoom_CartItems_CartItemsId",
                        column: x => x.CartItemsId,
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemRoom_RoomsId",
                table: "CartItemRoom",
                column: "RoomsId");
        }
    }
}
