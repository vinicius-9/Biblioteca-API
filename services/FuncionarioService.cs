using Biblioteca.Data;
using Biblioteca.Dtos;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Services
{
    // Regras de negócio do Funcionario
    public class FuncionarioService
    {
        // Acesso ao banco via EF Core
        private readonly AppDbContext _context;

        // DbContext vem por injeção de dependência
        public FuncionarioService(AppDbContext context)
        {
            _context = context;
        }

        // Cria Pessoa + Funcionario
        public async Task<FuncionarioResponse> CriarFuncionarioAsync(FuncionarioRequest request)
        {
            // CPF deve ser único
            if (await _context.Pessoas.AnyAsync(p => p.Cpf == request.Cpf))
                throw new Exception("CPF já cadastrado.");

            // Email usado como login deve ser único
            if (await _context.Funcionarios.AnyAsync(f => f.Email == request.Email))
                throw new Exception("Email já cadastrado.");

            // Criação da Pessoa (entidade base)
            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Cpf = request.Cpf
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync(); // gera Pessoa.Id

            // Criação do Funcionario ligado à Pessoa
            var funcionario = new Funcionario
            {
                PessoaId = pessoa.Id,
                Email = request.Email,
                Cargo = request.Cargo,
                SenhaHash = GerarHash(request.Senha), // senha nunca em texto puro
                Ativo = true
            };

            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            // Retorno via DTO
            return new FuncionarioResponse
            {
                Id = funcionario.Id,
                Nome = pessoa.Nome,
                Cpf = pessoa.Cpf,
                Email = funcionario.Email,
                Cargo = funcionario.Cargo,
                Ativo = funcionario.Ativo
            };
        }

        // Lista todos os funcionarios
        public async Task<List<FuncionarioResponse>> ObterTodosAsync()
        {
            var funcionarios = await _context.Funcionarios
                .Include(f => f.Pessoa) // traz dados da Pessoa
                .ToListAsync();

            return funcionarios.Select(f => new FuncionarioResponse
            {
                Id = f.Id,
                Nome = f.Pessoa.Nome,
                Cpf = f.Pessoa.Cpf,
                Email = f.Email,
                Cargo = f.Cargo,
                Ativo = f.Ativo
            }).ToList();
        }

        // Busca funcionario por Id
        public async Task<FuncionarioResponse?> ObterPorIdAsync(int id)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.Pessoa)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionario == null) return null;

            return new FuncionarioResponse
            {
                Id = funcionario.Id,
                Nome = funcionario.Pessoa.Nome,
                Cpf = funcionario.Pessoa.Cpf,
                Email = funcionario.Email,
                Cargo = funcionario.Cargo,
                Ativo = funcionario.Ativo
            };
        }

        // Gera hash da senha
        private static string GerarHash(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }
    }
}
