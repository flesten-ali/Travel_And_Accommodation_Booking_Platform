using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TABP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeThumbnailUrlcol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Images_ThumbnailId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ThumbnailId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Cities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailId",
                table: "Hotels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailId",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ThumbnailId",
                table: "Hotels",
                column: "ThumbnailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Images_ThumbnailId",
                table: "Hotels",
                column: "ThumbnailId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
