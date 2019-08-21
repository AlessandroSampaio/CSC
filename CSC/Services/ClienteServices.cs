using CSC.Models;
using CSC.Models.ErrorModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSC.Services
{
    public class ClienteServices
    {
        private readonly CSCContext _context;

        public ClienteServices(CSCContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> FindAllAsync()
        {
            return await _context.Cliente.OrderBy(c => c.RazaoSocial).ToListAsync();
        }

        public async Task<Cliente> FindByDocAsync(string _doc)
        {
            return await _context.Cliente.Where(c => c.CNPJ == _doc).FirstOrDefaultAsync();
        }

        public async Task<List<Cliente>> FindByNameAsync(string _name)
        {
            return await _context.Cliente.Where(c => c.RazaoSocial.Contains(_name)).OrderBy(c => c.RazaoSocial).ToListAsync();
        }

        public async Task<List<Inventario>> FindInventariosAsync(int id)
        {
            return await _context.Inventario.Where(i => i.ClienteID == id)
                .Include(c => c.Cliente)
                .ToListAsync();
        }

        public async Task RemoveInventario(Inventario inv)
        {
            _context.Inventario.Remove(inv);
            await _context.SaveChangesAsync();
        }

        public async Task SaveInventario(Inventario inventario)
        {
            try
            {
                if (_context.Inventario.Any(x => x.Cliente == inventario.Cliente && x.Software == inventario.Software))
                {

                    _context.Update(inventario);

                }
                else
                {
                    _context.Inventario.Add(inventario);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw e;
            }
        }

        public async Task<Cliente> FindByIdAsync(int id)
        {
            return await _context.Cliente
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public void Insert(Cliente cliente)
        {
            try
            {
                _context.Cliente.Add(cliente);
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw e;
            }
        }

        public async Task UpdateAsync(Cliente obj)
        {
            bool hasAny = await _context.Cliente.AnyAsync(x => x.Id == obj.Id);
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw e;
            }
        }


        public async Task<Cliente> ConsultaWS(string _cnpj)
        {
            _cnpj = _cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            string endpoint = "https://www.receitaws.com.br/v1/cnpj/" + _cnpj;

            string JsonRetorno = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(endpoint).Result;
                using (HttpContent content = response.Content)
                {


                    JsonRetorno = await content.ReadAsStringAsync();

                    ErrorModel error = JsonConvert.DeserializeObject<ErrorModel>(JsonRetorno);
                    if (error.status == "ERROR")
                    {
                        throw new NotImplementedException(error.message);
                    }
                    return JsonConvert.DeserializeObject<Cliente>(JsonRetorno);
                }
            }
        }
    }
}
