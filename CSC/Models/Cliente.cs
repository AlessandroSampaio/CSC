using CSC.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "Tamanho inválido" )]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números")]
        public string CNPJ { get; set; }

        [Required]
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [Required]
        [Display(Name = "Fantasia")]
        public string NomeFantasia { get; set; }

        [Display(Name = "Data de Contrato")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        public PessoaStatus Status { get; set; }

        public string Logradouro { get; set; }

        [StringLength(9,ErrorMessage ="CEP INVALIDO")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números")]
        public string CEP { get; set; }

        public int Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }
    }
}
