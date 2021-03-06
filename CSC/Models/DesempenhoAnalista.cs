﻿using CSC.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CSC.Models
{
    public class DesempenhoAnalista
    {
        public int AnalistaId { get; set; }
        public string Analista { get; set; }
        public int TotalAtendimento { get; set; }
        public int TotalAtendimentoAberto { get; set; }
        public int TotalAtendimentoFechado { get; set; }
        public int TotalAtendimentoTransferido { get; set; }
        public int TotalDias { get; set; }
        public decimal MediaAtendimento { get; set; }
        public int Chaves { get; set; }
        public int Operacional { get; set; }
        public int Tecnico { get; set; }
        public int Externo { get; set; }

        public DesempenhoAnalista() { }

        public DesempenhoAnalista(List<Atendimento> atendimentos, int Dias)
        {
            TotalAtendimento = atendimentos.Count;
            Analista = atendimentos.Select(s => s.User.Nome).First();
            AnalistaId = atendimentos.Select(s => s.User.UserId).First();
            TotalAtendimentoAberto = atendimentos.Where(s => s.Status == AtendimentoStatus.Aberto).Count();
            TotalAtendimentoTransferido = atendimentos.Where(s => s.Status == AtendimentoStatus.Transferido).Count();
            TotalDias = Dias;
            MediaAtendimento = Dias == 0 ? atendimentos.Count : atendimentos.Count / (decimal)Dias;
            Chaves = atendimentos.Where(t => t.AtendimentoTipo == TipoAtendimento.Chave).Count();
            Operacional = atendimentos.Where(t => t.AtendimentoTipo == TipoAtendimento.Operacional).Count();
            Tecnico = atendimentos.Where(t => t.AtendimentoTipo == TipoAtendimento.Tecnico).Count();
            Externo = atendimentos.Where(t => t.AtendimentoTipo == TipoAtendimento.Externo).Count();
        }
    }
}
