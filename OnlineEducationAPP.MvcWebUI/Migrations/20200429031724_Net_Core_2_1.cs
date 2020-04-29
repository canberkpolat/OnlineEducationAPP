using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineEducationAPP.MvcWebUI.Migrations
{
    public partial class Net_Core_2_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Streams_AspNetUsers_UserId",
                table: "Streams");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Streams",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_AspNetUsers_UserId",
                table: "Streams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Streams_AspNetUsers_UserId",
                table: "Streams");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Streams",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Streams_AspNetUsers_UserId",
                table: "Streams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
