using CSC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public User ValidUser(string _name, string _senha)
        {
            User user = new User(_name, _senha);
            if(_context.User.Where(u => u.nomeLogon==_name && u.Senha==getHash(_senha)) == null)
            {
                return null;
            }
            else
            {
                return _context.User.Where(u => u.nomeLogon == _name && u.Senha == getHash(_senha)).FirstOrDefault();
            }
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.User
                .FirstOrDefaultAsync(f => f.Id == id);
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
