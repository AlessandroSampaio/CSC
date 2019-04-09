using CSC.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public DateTime DataInicio { get; set; }
        public PessoaStatus Status { get; set; }
        public string Logradouro { get; set; }
        public string CEP { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }




    }
}
