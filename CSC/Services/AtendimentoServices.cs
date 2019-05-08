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
            return _context.Atendimento.ToListAsync();
        }
    }
}
