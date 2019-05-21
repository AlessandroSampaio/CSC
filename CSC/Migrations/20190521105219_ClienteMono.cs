using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class ClienteMono : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Mono",
                table: "Cliente",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mono",
                table: "Cliente");
        }
    }
}
