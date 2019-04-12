using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class ModelFuncionarioAlterado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Veiculo",
                table: "Funcionario",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Veiculo",
                table: "Funcionario",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
