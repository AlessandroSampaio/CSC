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
                if (plainText == null)
                    plainText = "";
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public User ValidUser(User user)
        {
            return _context.User.Where(u => u.NomeLogon == user.NomeLogon && u.Senha == GetHash(user.Senha))
                .FirstOrDefault();
        }

        public async Task<User> FindByFuncionarioIdAsync(int id)
        {
            return await _context.User.Where(u => u.FuncionarioId == id).FirstOrDefaultAsync();
        }

        public async Task RemoveUserAsync(User user)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.User
                .Include(user => user.Funcionario)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<User>> FindAllAsync()
        {
            return await _context.User
                .Include(user => user.Funcionario)
                .ToListAsync();
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

        public async Task UpdateAsync(User obj)
        {
            bool hasAny = await _context.User.AnyAsync(x => x.Id == obj.Id);
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw e;
            }
        }
    }
}
