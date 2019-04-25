using CSC.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public async Task<List<Cliente>> FindByNameAsync(string _name)
        {
            return await _context.Cliente.Where(c => c.RazaoSocial.Contains(_name)).OrderBy(c => c.RazaoSocial).ToListAsync();
        }

        public async Task InsertAsync(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public Cliente ConsultaWS(string _cnpj)
        {
            _cnpj = _cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            string endpoint = "https://www.receitaws.com.br/v1/cnpj/" + _cnpj;
            string method = "GET";

            HttpWebRequest request = WebRequest.CreateHttp(endpoint);
            request.Method = method;

            using (StreamReader responseStream = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string dadosRecuperados = responseStream.ReadToEnd();
                return JsonConvert.DeserializeObject<Cliente>(dadosRecuperados);
            }
        }

        private Cliente Json(string v)
        {
            throw new NotImplementedException();
        }
    }
}
