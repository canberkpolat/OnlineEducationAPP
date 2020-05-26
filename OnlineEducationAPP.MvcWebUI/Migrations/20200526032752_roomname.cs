using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineEducationAPP.MvcWebUI.Migrations
{
    public partial class roomname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Messages");
        }
    }
}
