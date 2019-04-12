using CSC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public string GetHash(string plainText)
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
            if(_context.User.Where(u => u.NomeLogon==_name && u.Senha==GetHash(_senha)) == null)
            {
                return null;
            }
            else
            {
                return _context.User.Where(u => u.NomeLogon == _name && u.Senha == GetHash(_senha)).FirstOrDefault();
            }
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.User
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<User>> FindAllAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<List<User>> FindByNameAsync(string _name)
        {
            return await _context.User.Where(user => user.NomeLogon.Contains(_name))
                .Include(user => user.Funcionario)
                .ToListAsync();
            
        }

        public async Task InsertUserAsync(User user)
        {
            user.Senha = GetHash(user.Senha);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

    }
}
