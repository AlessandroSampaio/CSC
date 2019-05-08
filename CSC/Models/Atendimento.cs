using CSC.Models.Enums;
using System;

namespace CSC.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public Funcionario Funcionario { get; set; }
        public int FuncionarioId { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public DateTime Abertura { get; set; }
        public DateTime Encerramento { get; set; }
        public AtendimentoStatus Status { get; set; }
        public TipoAtendimento AtendimentoTipo { get; set; }
        public int? OrigemID { get; set; }
        public string Detalhes { get; set; }
    }
}
