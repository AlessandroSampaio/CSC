using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Usuario")]
        public string NomeLogon { get; set; }
        public string Senha { get; set; }
        [Required]
        public Funcionario Funcionario { get; set; }

        public User() { }


        public User(string name, string senha)
        {
            this.NomeLogon = name;
            this.Senha = senha;
        }
    }
}
