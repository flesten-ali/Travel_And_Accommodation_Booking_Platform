using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeunneededfksinimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_RoomClasses_RoomClassId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_HotelId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_RoomClassId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "RoomClassId",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomClassId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_HotelId",
                table: "Images",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_RoomClassId",
                table: "Images",
                column: "RoomClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelId",
                table: "Images",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_RoomClasses_RoomClassId",
                table: "Images",
                column: "RoomClassId",
                principalTable: "RoomClasses",
                principalColumn: "Id");
        }
    }
}
