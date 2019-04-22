using CSC.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(14)]
        [MinLength(11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Somente números")]
        public string CNPJ { get; set; }
        [Required]
        public string RazaoSocial { get; set; }
        [Required]
        public string NomeFantasia { get; set; }
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
