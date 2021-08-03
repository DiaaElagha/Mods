using Microsoft.EntityFrameworkCore.Migrations;

namespace SkinsAdmin.Migrations
{
    public partial class addmod_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModUrl",
                table: "Mods",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModUrl",
                table: "Mods");
        }
    }
}
