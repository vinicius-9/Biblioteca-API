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
        public DbSet<Livro> Livros { get; set; }
        
        public DbSet<Emprestimo> Emprestimos { get; set; }

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

            
            //Impedir que dois Clientes aponte para mesma Pessoa
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.PessoaId)
                .IsUnique();

            // Relacionamento 1-1 Pessoa <-> Funcionario
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Pessoa)
                .WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(f => f.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            //Impedir que dois Funcionarios aponte para a mesma Pessoa
            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.PessoaId)
                .IsUnique();
          

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Livro)
                .WithMany()
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Emprestimo>()
               .HasOne(e => e.Cliente)
               .WithMany()
               .HasForeignKey(e => e.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Emprestimo>()
               .HasOne(e => e.Funcionario)
               .WithMany()
               .HasForeignKey(e => e.FuncionarioId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
