using System;

namespace CSC.Models.ViewModel
{
    public class UserIndexViewModel
    {
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string UserName { get; set; }
        public DateTime Admissao { get; set; }

        public UserIndexViewModel(User user)
        {
            UserId = user.UserId;
            Nome = user.Nome;
            UserName = user.UserName;
            Admissao = user.Admissao;
        }
    }
}
