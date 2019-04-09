using CSC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CSC.Services
{
    public class FuncionarioServices
    {
        private readonly CSCContext _context;

        public FuncionarioServices(CSCContext context)
        {
            _context = context;
        }

        public IEnumerable<Funcionario> FindAll()
        {
            return _context.Funcionario.ToList();
        }

        public async Task<List<Funcionario>> FindByNameAsync(string nome)
        {
            return await _context.Funcionario.Where(f => f.Nome.Contains(nome))
                .OrderBy(f => f.Nome)
                .ToListAsync();
        }

        public List<Funcionario> FindByName(string nome)
        {
            return _context.Funcionario.Where(f => f.Nome.Contains(nome))
                .OrderBy(f => f.Nome)
                .ToList();
        }

        public async Task<Funcionario> FindByIdAsync(int id)
        {
            return await _context.Funcionario
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task InsertAsync(Funcionario func)
        {
            _context.Funcionario.Add(func);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Funcionario obj)
        {
            bool hasAny = await _context.Funcionario.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                //throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                //throw new DbConcurrencyException(e.Message);
            }
        }

    }
}
