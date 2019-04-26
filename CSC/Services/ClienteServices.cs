using CSC.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public async Task<List<Cliente>> FindByNameAsync(string _name)
        {
            return await _context.Cliente.Where(c => c.RazaoSocial.Contains(_name)).OrderBy(c => c.RazaoSocial).ToListAsync();
        }

        public async Task InsertAsync(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente> ConsultaWS(string _cnpj)
        {
            _cnpj = _cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            string endpoint = "https://www.receitaws.com.br/v1/cnpj/" + _cnpj;
            
            string JsonRetorno = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(endpoint).Result;
                using(HttpContent content = response.Content)
                {
                    

                    JsonRetorno = await content.ReadAsStringAsync();

                    Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonRetorno);
                    if(dic["status"] == "ERROR")
                    {
                        throw new NotImplementedException(dic["message"]);
                    }
                    return JsonConvert.DeserializeObject<Cliente>(JsonRetorno);
                }
            }            
        }
    }
}
