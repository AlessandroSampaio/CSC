using CSC.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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

        [JsonProperty("numero")]
        public int? Numero { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("municipio")]
        public string Cidade { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [NotMapped]
        public string dataInicio
        {
            get { return DataInicio.Day < 10 ? '0' + DataInicio.ToString("dd/MM/yyyy") : DataInicio.ToString("dd/MM/yyyy"); }
            set { DataInicio = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture); }
        }
    }
}
