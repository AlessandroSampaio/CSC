namespace CSC.Models.ViewModel
{
    public class PasswordResetViewModel
    {
        public string UserId { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string ConfirmSenha { get; set; }
        public string Token { get; set; }
    }
}
