using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string nomeLogon { get; set; }
        public string Senha { get; set; }
        public Funcionario funcionario { get; set; }

        public User() { }


        public User(string name, string senha)
        {
            this.nomeLogon = name;
            this.Senha = senha;
        }
    }
}
