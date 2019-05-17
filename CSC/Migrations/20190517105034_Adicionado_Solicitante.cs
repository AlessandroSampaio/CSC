using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class Adicionado_Solicitante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atendimento_Cliente_ClienteId",
                table: "Atendimento");

            migrationBuilder.DropForeignKey(
                name: "FK_Atendimento_Funcionario_FuncionarioId",
                table: "Atendimento");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Atendimento",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Atendimento",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Solicitante",
                table: "Atendimento",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Atendimento_Cliente_ClienteId",
                table: "Atendimento",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Atendimento_Funcionario_FuncionarioId",
                table: "Atendimento",
                column: "FuncionarioId",
                principalTable: "Funcionario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atendimento_Cliente_ClienteId",
                table: "Atendimento");

            migrationBuilder.DropForeignKey(
                name: "FK_Atendimento_Funcionario_FuncionarioId",
                table: "Atendimento");

            migrationBuilder.DropColumn(
                name: "Solicitante",
                table: "Atendimento");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Atendimento",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Atendimento",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Atendimento_Cliente_ClienteId",
                table: "Atendimento",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Atendimento_Funcionario_FuncionarioId",
                table: "Atendimento",
                column: "FuncionarioId",
                principalTable: "Funcionario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
