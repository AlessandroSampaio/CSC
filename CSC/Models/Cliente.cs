using CSC.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSC.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " ")]
        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }

        [Required]
        [JsonProperty("nome")]
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [Required]
        [JsonProperty("fantasia")]
        [Display(Name = "Fantasia")]
        public string NomeFantasia { get; set; }

        [Display(Name = "Data de Contrato")]
        public DateTime DataInicio { get; set; }

        [JsonProperty("SituacaoCadastro")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PessoaStatus Status { get; set; }

        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty("cep")]
        public string CEP { get; set; }

        [JsonIgnore]
        public int? Numero { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("municipio")]
        public string Cidade { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool Mono { get; set; }

        [JsonProperty("numero")]
        [NotMapped]
        public string numero
        {
            get { return Numero.ToString(); }
            set
            {
                if (value.Contains("S") || value == null || value.Equals("")) { Numero = 0; }
                else
                {
                    Numero = Convert.ToInt32(value);
                }
            }
        }
    }
}
