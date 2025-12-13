using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pessoa.Cpf único
            modelBuilder.Entity<Pessoa>()
                .HasIndex(p => p.Cpf)
                .IsUnique();

            // Relacionamento 1-1 Pessoa <-> Cliente
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Pessoa)
                .WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(c => c.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento 1-1 Pessoa <-> Funcionario
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Pessoa)
                .WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(f => f.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
