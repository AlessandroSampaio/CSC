using Microsoft.EntityFrameworkCore;

namespace CSC.Models
{
    public class CSCContext : DbContext
    {
        public CSCContext(DbContextOptions<CSCContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventario>()
                .HasKey(i => new { i.ClienteID, i.Software });
        }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
    }
}