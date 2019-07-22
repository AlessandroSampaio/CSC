using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class NewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CNPJ = table.Column<string>(nullable: false),
                    RazaoSocial = table.Column<string>(nullable: false),
                    NomeFantasia = table.Column<string>(nullable: false),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Logradouro = table.Column<string>(nullable: true),
                    CEP = table.Column<string>(nullable: true),
                    Numero = table.Column<int>(nullable: true),
                    Bairro = table.Column<string>(nullable: true),
                    Cidade = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mono = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Admissao = table.Column<DateTime>(nullable: false),
                    Demissao = table.Column<DateTime>(nullable: true),
                    Veiculo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TarefaNumero = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: false),
                    Conclusao = table.Column<DateTime>(nullable: true),
                    Abertura = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    ClienteID = table.Column<int>(nullable: false),
                    Software = table.Column<int>(nullable: false),
                    Quantidade = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => new { x.ClienteID, x.Software });
                    table.ForeignKey(
                        name: "FK_Inventario_Cliente_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeLogon = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    FuncionarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Atendimento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FuncionarioId = table.Column<int>(nullable: false),
                    ClienteId = table.Column<int>(nullable: false),
                    Abertura = table.Column<DateTime>(nullable: false),
                    Encerramento = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    AtendimentoTipo = table.Column<int>(nullable: false),
                    OrigemID = table.Column<int>(nullable: true),
                    Detalhes = table.Column<string>(nullable: true),
                    Solicitante = table.Column<string>(nullable: true),
                    TarefaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atendimento_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimento_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimento_Tarefa_TarefaId",
                        column: x => x.TarefaId,
                        principalTable: "Tarefa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Funcionario",
                columns: new[] { "Id", "Admissao", "Demissao", "Nome", "Veiculo" },
                values: new object[] { 1, new DateTime(2019, 7, 22, 0, 0, 0, 0, DateTimeKind.Local), null, "Admin", false });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FuncionarioId", "NomeLogon", "Senha" },
                values: new object[] { 1, 1, "admin", "2c84d6ef7d7d16c82eba6487caae5247b45d08555b45d0bc43af625def92a8d6" });

            migrationBuilder.CreateIndex(
                name: "IX_Atendimento_ClienteId",
                table: "Atendimento",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimento_FuncionarioId",
                table: "Atendimento",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimento_TarefaId",
                table: "Atendimento",
                column: "TarefaId");

            migrationBuilder.CreateIndex(
                name: "IX_User_FuncionarioId",
                table: "User",
                column: "FuncionarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atendimento");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Tarefa");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Funcionario");
        }
    }
}
