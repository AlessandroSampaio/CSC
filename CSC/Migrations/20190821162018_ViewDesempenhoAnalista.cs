using Microsoft.EntityFrameworkCore.Migrations;

namespace CSC.Migrations
{
    public partial class ViewDesempenhoAnalista : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"CREATE OR REPLACE VIEW `desempenho_analista` AS " +
                "SELECT `u`.`UserId` AS `AnalistaId`,`u`.`Nome` AS `Analista`, COUNT(1) AS `TotalAtendimento`, " +
                "SUM((CASE WHEN(`a`.`Status` = 0) THEN 1 ELSE 0 END)) AS `TotalAtendimentoAberto`, " +
                "SUM((CASE WHEN(`a`.`Status` = 1) THEN 1 ELSE 0 END)) AS `TotalAtendimentoFechado`, " +
                "SUM((CASE WHEN(`a`.`Status` = 2) THEN 1 ELSE 0 END)) AS `TotalAtendimentoTransferido`, " +
                "((TO_DAYS(MAX(`a`.`Abertura`)) - TO_DAYS(MIN(`a`.`Abertura`))) + 1) AS `TotalDias`, " +
                "(COUNT(1) / ((TO_DAYS(MAX(`a`.`Abertura`)) - TO_DAYS(MIN(`a`.`Abertura`))) + 1)) AS `MediaAtendimento`, " +
                "SUM((CASE WHEN(`a`.`AtendimentoTipo` = 0) THEN 1 ELSE 0 END)) AS `chaves`, " +
                "SUM((CASE WHEN(`a`.`AtendimentoTipo` = 2) THEN 1 ELSE 0 END)) AS `operacional`, " +
                "SUM((CASE WHEN(`a`.`AtendimentoTipo` = 1) THEN 1 ELSE 0 END)) AS `tecnico`, " +
                "SUM((CASE WHEN(`a`.`AtendimentoTipo` = 3) THEN 1 ELSE 0 END)) AS `externo` " +
                "FROM (`atendimento` `a` JOIN `aspnetusers` `u` ON((`u`.`Id` = `a`.`UserId`))) GROUP BY `a`.`UserId`", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view desempenho_analista", false);
        }
    }
}
