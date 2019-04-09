using CSC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
