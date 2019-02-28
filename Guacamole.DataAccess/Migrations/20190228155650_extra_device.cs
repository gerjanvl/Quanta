using Microsoft.EntityFrameworkCore.Migrations;

namespace Guacamole.DataAccess.Migrations
{
    public partial class extra_device : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Devices",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "Devices");
        }
    }
}
