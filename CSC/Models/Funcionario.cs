using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [DataType (DataType.Date)]
        public DateTime Admissao  { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Demissao { get; set; }
        public char Veiculo { get; set; }

    }
}
