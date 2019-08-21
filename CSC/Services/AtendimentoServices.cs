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
                .Include(f => f.User)
                .ToListAsync();
        }

        public Task<List<Atendimento>> FindAllAsync(AtendimentoStatus status)
        {
            return _context.Atendimento
                .Include(c => c.Cliente)
                .Include(f => f.User)
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<Atendimento> FindByIDAsync(int id)
        {
            return await _context.Atendimento.Where(a => a.Id == id)
                .Include(c => c.Cliente)
                .Include(f => f.User)
                .FirstOrDefaultAsync();
        }

        public Task<List<Atendimento>> FindByClientAsync(int id)
        {
            return _context.Atendimento
                .Where(c => c.ClienteId== id)
                .Include(c => c.Cliente)
                .Include(f => f.User)
                .ToListAsync();
        }

        public Task<List<Atendimento>> FindByFuncinoarioAsync(int Userid)
        {
            return _context.Atendimento
                            .Include(c => c.Cliente)
                            .Include(f => f.User)
                            .Where(f => f.User.UserId == Userid)
                            .ToListAsync();
        }

        public Task<List<Atendimento>> FindByDateIntervalAsync(DateTime dataInicio, DateTime dataFim)
        {
            return _context.Atendimento
                            .Where(a => a.Abertura >= dataInicio && a.Abertura <= dataFim)
                            .Include(c => c.Cliente)
                            .Include(f => f.User)
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

        public async Task<List<DesempenhoAnalista>> GetDesempenhoAnalistas(DateTime? inicio, DateTime? fim)
        {
            //Verifica se foi fornecido valores de Data e retorna o intervalo
            if(inicio != null && fim != null)
            {
                return await _context.DesempenhoAnalista.FromSql($"select * from desempenho_analista where abertura between {0} and {1}", inicio, fim).ToListAsync();
            }
            //Caso não informado, retorna toda a view
            if(inicio == null && fim == null)
            {
                return await _context.DesempenhoAnalista.FromSql($"select * from desempenho_analista").ToListAsync();
            }
            return null;
        }
        
        public async Task<List<Atendimento>> TotalizacaoAtendimentosAsync()
        {
            return await _context.Atendimento.ToListAsync();
        }
    }
}
