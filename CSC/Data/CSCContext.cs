using Microsoft.EntityFrameworkCore;
using CSC.Services;
using System;

namespace CSC.Models
{
    public class CSCContext : DbContext
    {
        public CSCContext(DbContextOptions<CSCContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventario>()
                .HasKey(i => new { i.ClienteID, i.Software });
            modelBuilder.Entity<Funcionario>().HasData(new Funcionario(1, "Admin", DateTime.Today));
            modelBuilder.Entity<User>().HasData(new User(1,"admin", UserServices.GetHash("G@_Rei_0!"), 1));
        }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Atendimento> Atendimento { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
    }
}