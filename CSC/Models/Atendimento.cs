﻿using CSC.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public DateTime Abertura { get; set; }
        public DateTime Encerramento { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AtendimentoStatus Status { get; set; }
        [Display(Name = "Tipo")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TipoAtendimento AtendimentoTipo { get; set; }
        public int? OrigemID { get; set; }
        public string Detalhes { get; set; }
        public string Solicitante { get; set; }
        public int? TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }

        public Atendimento()
        {

        }
    }
}
