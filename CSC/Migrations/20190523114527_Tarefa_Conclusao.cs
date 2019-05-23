using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class Tarefa_Conclusao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Conclusao",
                table: "Tarefa",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TarefaNumero",
                table: "Tarefa",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conclusao",
                table: "Tarefa");

            migrationBuilder.DropColumn(
                name: "TarefaNumero",
                table: "Tarefa");
        }
    }
}
