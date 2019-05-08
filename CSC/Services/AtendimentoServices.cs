using CSC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSC.Services
{
    public class AtendimentoServices
    {
        private readonly CSCContext _context;

        public AtendimentoServices(CSCContext context)
        {
            _context = context;
        }

        public Task<List<Atendimento>> FindAllAsync()
        {
            return _context.Atendimento
                .Include(c => c.Cliente)
                .Include(f => f.Funcionario)
                .ToListAsync();
        }

        public async Task InsertAsync(Atendimento atendimento)
        {
            _context.Atendimento.Add(atendimento);
            await _context.SaveChangesAsync();
        }
    }
}
