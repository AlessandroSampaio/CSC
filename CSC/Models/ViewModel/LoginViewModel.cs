using System.ComponentModel.DataAnnotations;

namespace CSC.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Logon { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Continuar Conectado?")]
        public bool RememberMe { get; set; }
    }
}
