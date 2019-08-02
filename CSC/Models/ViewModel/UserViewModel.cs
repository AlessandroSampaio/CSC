using CSC.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Logon")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo deve ser preenchido")]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "Data de Admissão")]
        [DataType(DataType.Date)]
        public DateTime Admissao { get; set; }
        [Display(Name = "Data de Demissão")]
        [DataType(DataType.Date)]
        public DateTime? Demissao { get; set; }
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; }
        [Display(Name = "Cargo")]
        public Roles Role { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Nome = user.Nome;
            Admissao = user.Admissao;
            Demissao = user.Demissao;
            Email = user.Email;
        }
        public UserViewModel() { }

        public UserViewModel(User user, Roles role)
        {
            Role = role;
            Id = user.Id;
            Nome = user.Nome;
            Email = user.Email;
            Admissao = user.Admissao;
            Demissao = user.Demissao;
            UserName = user.UserName;
        }
    }
}
