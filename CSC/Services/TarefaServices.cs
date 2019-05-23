using CSC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSC.Services
{
    public class TarefaServices
    {
        private readonly CSCContext _context;

        public TarefaServices(CSCContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> FindAllAsync()
        {
            return await _context.Tarefa.ToListAsync();
        }

        public async Task InsertAsync(Tarefa tarefa)
        {
            _context.Tarefa.Add(tarefa);
            await _context.SaveChangesAsync();
        }
    }
}
