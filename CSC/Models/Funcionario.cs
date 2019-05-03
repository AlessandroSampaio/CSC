using System;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo deve ser preenchido")]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "Data de Admissão")]
        [DataType(DataType.Date)]
        public DateTime Admissao { get; set; }
        [Display(Name = "Data de Demissão")]
        [DataType(DataType.Date)]
        public DateTime? Demissao { get; set; }
        public bool Veiculo { get; set; }

    }
}
