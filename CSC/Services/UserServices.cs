using CSC.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSC.Services
{
    public class UserServices
    {

        private readonly CSCContext _context;

        public UserServices(CSCContext context)
        {
            _context = context;
        }

        public string getHash(string plainText)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        public async Task InsertUserAsync(User user)
        {
            string plainText = user.Senha;
            user.Senha = getHash(plainText);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

    }
}
