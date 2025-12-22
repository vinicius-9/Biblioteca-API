using Biblioteca.Data;
using Biblioteca.Dtos;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class EmprestimoService
    {
        private readonly AppDbContext _context;

        public EmprestimoService(AppDbContext context)
        {
            _context = context;
        }

        // CRIAR EMPRÉSTIMO
        public async Task<EmprestimoResponse> CriarAsync(EmprestimoRequest request)
        {
            // Verifica livro
            var livro = await _context.Livros
                .FirstOrDefaultAsync(l => l.Id == request.LivroId && l.Ativo);

            if (livro == null)
                throw new Exception("Livro não encontrado ou inativo.");

            // Verifica cliente
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == request.ClienteId && c.Ativo);

            if (cliente == null)
                throw new Exception("Cliente não encontrado ou inativo.");

            // Verifica funcionário
            var funcionario = await _context.Funcionarios
                .Include(f => f.Pessoa)
                .FirstOrDefaultAsync(f => f.Id == request.FuncionarioId && f.Ativo);

            if (funcionario == null)
                throw new Exception("Funcionário não encontrado ou inativo.");

          
            // Impede emprestar livro já emprestado e não devolvido
            var livroJaEmprestado = await _context.Emprestimos
                .AnyAsync(e => e.LivroId == request.LivroId && e.DataDevolucao == null);

            if (livroJaEmprestado)
                throw new Exception("Este livro já está emprestado.");

           var emprestimo = new Emprestimo
           {
               LivroId = request.LivroId,
               ClienteId = request.ClienteId,
               FuncionarioId = request.FuncionarioId,
               DataEmprestimo = request.DataEmprestimo
           };


            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();

            return await ObterPorIdAsync(emprestimo.Id)
                   ?? throw new Exception("Erro ao criar empréstimo.");
        }

        // Busca emprestimo por Id
        public async Task<EmprestimoResponse?> ObterPorIdAsync(int id)
        {
            var e = await _context.Emprestimos
                .Include(x => x.Livro)
                .Include(x => x.Funcionario)
                    .ThenInclude(f => f.Pessoa)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return null;

            return new EmprestimoResponse
            {
                Id = e.Id,
                LivroTitulo = e.Livro.Titulo,
                LivroAno = e.Livro.AnoPublicacao,
                FuncionarioNome = e.Funcionario.Pessoa.Nome,
                FuncionarioCargo = e.Funcionario.Cargo,
                DataEmprestimo = e.DataEmprestimo,
                DataDevolucao = e.DataDevolucao
            };
        }

        // Atualiza data de devolucao
        public async Task<bool> AtualizarDataDevolucaoAsync(int id, DateTime dataDevolucao)
        {
            var emprestimo = await _context.Emprestimos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (emprestimo == null)
                return false;

            emprestimo.DataDevolucao = dataDevolucao;
            await _context.SaveChangesAsync();

            return true;
        }

        // Lista todos os emprestimos
        public async Task<List<EmprestimoResponse>> ObterTodosAsync()
        {
            var emprestimos = await _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Funcionario)
                    .ThenInclude(f => f.Pessoa)
                .ToListAsync();

            return emprestimos.Select(e => new EmprestimoResponse
            {
                Id = e.Id,
                LivroTitulo = e.Livro.Titulo,
                LivroAno = e.Livro.AnoPublicacao,
                FuncionarioNome = e.Funcionario.Pessoa.Nome,
                FuncionarioCargo = e.Funcionario.Cargo,
                DataEmprestimo = e.DataEmprestimo,
                DataDevolucao = e.DataDevolucao
            }).ToList();
        }
    }
}
