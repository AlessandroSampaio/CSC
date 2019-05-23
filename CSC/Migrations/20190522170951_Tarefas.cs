using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class Tarefas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TarefaId",
                table: "Atendimento",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefa", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atendimento_TarefaId",
                table: "Atendimento",
                column: "TarefaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atendimento_Tarefa_TarefaId",
                table: "Atendimento",
                column: "TarefaId",
                principalTable: "Tarefa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atendimento_Tarefa_TarefaId",
                table: "Atendimento");

            migrationBuilder.DropTable(
                name: "Tarefa");

            migrationBuilder.DropIndex(
                name: "IX_Atendimento_TarefaId",
                table: "Atendimento");

            migrationBuilder.DropColumn(
                name: "TarefaId",
                table: "Atendimento");
        }
    }
}
