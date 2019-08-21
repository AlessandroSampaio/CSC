using Microsoft.EntityFrameworkCore;
using CSC.Services;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CSC.Models
{
    public class CSCContext : IdentityDbContext<User>
    {
        public CSCContext(DbContextOptions<CSCContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventario>()
                .HasKey(i => new { i.ClienteID, i.Software });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Atendimento> Atendimento { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
        public DbQuery<DesempenhoAnalista> DesempenhoAnalista { get; set; }

    }
}