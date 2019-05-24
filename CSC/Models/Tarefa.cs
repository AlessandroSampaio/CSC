using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        [Display(Name = "Tarefa")]
        public string TarefaNumero { get; set; }
        [Required]
        public string Descricao { get; set; }
        public ICollection<Atendimento> Atendimentos { get; set; }
        public DateTime? Conclusao{ get; set; }

    }
}
