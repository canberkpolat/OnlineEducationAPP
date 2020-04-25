using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OnlineEducationAPP.MvcWebUI.Migrations
{
    public partial class stream_model_newfield_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endpoint",
                table: "Streams",
                newName: "VideoOnDemandEndpoint");

            migrationBuilder.AddColumn<string>(
                name: "LiveStreamEndpoint",
                table: "Streams",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Streams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiveStreamEndpoint",
                table: "Streams");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Streams");

            migrationBuilder.RenameColumn(
                name: "VideoOnDemandEndpoint",
                table: "Streams",
                newName: "Endpoint");
        }
    }
}
