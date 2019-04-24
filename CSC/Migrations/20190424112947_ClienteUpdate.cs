using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class ClienteUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CNPJ",
                table: "Cliente",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Cliente",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Cliente",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Cliente",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Cliente");

            migrationBuilder.AlterColumn<string>(
                name: "CNPJ",
                table: "Cliente",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Cliente",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9,
                oldNullable: true);
        }
    }
}
