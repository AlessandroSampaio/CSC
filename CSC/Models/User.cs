using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "O campo usuário não pode estar em branco!")]
        [Remote(action: "VerifyLogon", controller: "Home", AdditionalFields = nameof(Senha))]
        public string NomeLogon { get; set; }
        [Required(ErrorMessage = "O campo senha não pode estar em branco!")]
        public string Senha { get; set; }
        [Display(Name = "Funcionario")]
        public Funcionario Funcionario { get; set; }
        [Display(Name = "Funcionario")]
        public int FuncionarioId { get; set; }

        public User() { }


        public User(string name, string senha)
        {
            this.NomeLogon = name;
            this.Senha = senha;
        }
    }
}
