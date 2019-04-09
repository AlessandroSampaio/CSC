using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models
{
    public class CSCContext : DbContext
    {
        public CSCContext(DbContextOptions<CSCContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<User> User { get; set; }
    }
}
