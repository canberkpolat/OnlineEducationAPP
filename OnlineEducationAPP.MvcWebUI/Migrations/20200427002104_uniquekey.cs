using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineEducationAPP.MvcWebUI.Migrations
{
    public partial class uniquekey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streams_UserId",
                table: "Streams");

            migrationBuilder.CreateIndex(
                name: "IX_Streams_UserId",
                table: "Streams",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streams_UserId",
                table: "Streams");

            migrationBuilder.CreateIndex(
                name: "IX_Streams_UserId",
                table: "Streams",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
