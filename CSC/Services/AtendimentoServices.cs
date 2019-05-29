using CSC.Models;
using CSC.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public Task<List<Atendimento>> FindAllAsync(AtendimentoStatus status)
        {
            return _context.Atendimento
                .Include(c => c.Cliente)
                .Include(f => f.Funcionario)
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<Atendimento> FindByIDAsync(int id)
        {
            return await _context.Atendimento.Where(a => a.Id == id)
                .Include(c => c.Cliente)
                .Include(f => f.Funcionario)
                .FirstOrDefaultAsync();
        }

        public Task<List<Atendimento>> FindByClientAsync(int id)
        {
            return _context.Atendimento
                .Where(c => c.ClienteId== id)
                .Include(c => c.Cliente)
                .Include(f => f.Funcionario)
                .ToListAsync();
        }

        public async Task InsertAsync(Atendimento atendimento)
        {
            _context.Atendimento.Add(atendimento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Atendimento atendimento)
        {
            bool hasAny = _context.Atendimento.Any(a => a.Id == atendimento.Id);
            if(hasAny)
            {
                _context.Atendimento.Update(atendimento);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        
        public async Task<List<Atendimento>> TotalizacaoAtendimentosAsync()
        {
            return await _context.Atendimento.ToListAsync();
        }
    }
}
