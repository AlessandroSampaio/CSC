using CSC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Tarefa> FindByIdAsync(int id)
        {
            return await _context.Tarefa.Where(t => t.Id == id)
                .Include(a => a.Atendimentos)
                    .ThenInclude(c => c.Cliente)
                .Include(a => a.Atendimentos)
                    .ThenInclude(f => f.Funcionario)
                .FirstOrDefaultAsync();
        }

        public bool FindByTarefa(string TarefaNumero)
        {
            return  _context.Tarefa.Any(t => t.TarefaNumero == TarefaNumero);
        }

        public void Insert(Tarefa tarefa)
        {
            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
        }
    }
}
