using Biblioteca.Data;
using Biblioteca.Dtos;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Services
{
    // Classe que contém toda a lógica de negócio relacionada a Funcionários
    public class FuncionarioService
    {
        // DbContext para acessar o banco via Entity Framework Core
        private readonly AppDbContext _context;

        // Construtor recebe o DbContext por injeção de dependência
        public FuncionarioService(AppDbContext context)
        {
            _context = context;
        }

        // 1. CRIAÇÃO DE FUNCIONÁRIO
        public async Task<FuncionarioResponse> CriarFuncionarioAsync(FuncionarioRequest request)
        {
            // Verifica se já existe uma pessoa com o mesmo CPF
            if (await _context.Pessoas.AnyAsync(p => p.Cpf == request.Cpf))
                throw new Exception("CPF já cadastrado.");

            // Verifica se já existe um funcionário com o mesmo email
            if (await _context.Funcionarios.AnyAsync(f => f.Email == request.Email))
                throw new Exception("Email já cadastrado.");

            // Cria a Pessoa (dados básicos)
            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Cpf = request.Cpf
            };

            // Adiciona a pessoa ao contexto e salva para gerar Id
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            // Cria o Funcionário vinculado à Pessoa
            var funcionario = new Funcionario
            {
                PessoaId = pessoa.Id,
                Email = request.Email,
                Cargo = request.Cargo,
                SenhaHash = GerarHash(request.Senha), // nunca salvar senha em texto puro
                Ativo = true
            };

            // Adiciona o funcionário e salva no banco
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            // Retorna DTO para o controller (somente dados necessários)
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

        // 2. LISTAGEM DE FUNCIONÁRIOS
        public async Task<List<FuncionarioResponse>> ObterTodosAsync()
        {
            // Busca todos os funcionários incluindo os dados da Pessoa
            var funcionarios = await _context.Funcionarios
                .Include(f => f.Pessoa) // traz dados relacionados da Pessoa
                .ToListAsync();

            // Mapeia para DTO e retorna
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

        // BUSCAR FUNCIONÁRIO POR ID
        public async Task<FuncionarioResponse?> ObterPorIdAsync(int id)
        {
            // Busca o funcionário pelo Id, incluindo dados da Pessoa
            var funcionario = await _context.Funcionarios
                .Include(f => f.Pessoa)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionario == null) return null; // se não existir, retorna null

            // Retorna DTO com dados combinados de Pessoa + Funcionario
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

        // ATUALIZAÇÃO DE FUNCIONÁRIO (não altera CPF)
        public async Task<FuncionarioResponse> AtualizarFuncionarioAsync(int id, FuncionarioRequest request)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.Pessoa)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionario == null)
                throw new Exception("Funcionario nao encontrado");

            // Atualiza apenas dados que podem ser alterados
            funcionario.Pessoa.Nome = request.Nome; // nome pode ser atualizado
            funcionario.Email = request.Email;      // email pode ser atualizado
            funcionario.Cargo = request.Cargo;      // cargo pode ser atualizado

            // Atualiza a senha apenas se fornecida
            if (!string.IsNullOrWhiteSpace(request.Senha))
                funcionario.SenhaHash = GerarHash(request.Senha);

            await _context.SaveChangesAsync();

            return new FuncionarioResponse
            {
                Id = funcionario.Id,
                Nome = funcionario.Pessoa.Nome,
                Cpf = funcionario.Pessoa.Cpf, // CPF não é alterado
                Email = funcionario.Email,
                Cargo = funcionario.Cargo,
                Ativo = funcionario.Ativo
            };
        }

        // NOVO MÉTODO: Atualiza apenas a senha, com confirmação da senha antiga
        public async Task AtualizarSenhaAsync(int id, AtualizarSenhaRequest request)
        {
            var funcionario = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
            if (funcionario == null)
                throw new Exception("Funcionário não encontrado.");

            // Verifica se a senha antiga bate
            if (funcionario.SenhaHash != GerarHash(request.SenhaAntiga))
                throw new Exception("Senha antiga incorreta.");

            // Atualiza para a nova senha
            funcionario.SenhaHash = GerarHash(request.NovaSenha);
            await _context.SaveChangesAsync();
        }

        // MÉTODO PRIVADO PARA HASH DE SENHA
        private static string GerarHash(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }
    }

   
}
