using Biblioteca.Data;
using Biblioteca.Dtos;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    // Regras de negócio relacionadas a Livro
    public class LivroService
    {
        private readonly AppDbContext _context;

        public LivroService(AppDbContext context)
        {
            _context = context;
        }

        // Criar um livro
        public async Task<LivroResponse> CriarAsync(LivroRequest request)
        {
            var livro = new Livro
            {
                Titulo = request.Titulo,
                Autor = request.Autor,
                AnoPublicacao = request.AnoPublicacao
            };

            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            return new LivroResponse
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                AnoPublicacao = livro.AnoPublicacao,
                Ativo = livro.Ativo
            };
        }

        // Buscar livro por Id
        public async Task<LivroResponse?> ObterPorIdAsync(int id)
        {
            var livro = await _context.Livros
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (livro == null)
                return null;

            return new LivroResponse
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                AnoPublicacao = livro.AnoPublicacao,
                Ativo = livro.Ativo
            };
        }

        // Atualizar livro
        public async Task<bool> AtualizarAsync(int id, LivroRequest request)
        {
            var livro = await _context.Livros
                .FirstOrDefaultAsync(l => l.Id == id);

            if (livro == null)
                return false;

            livro.Titulo = request.Titulo;
            livro.Autor = request.Autor;
            livro.AnoPublicacao = request.AnoPublicacao;

            await _context.SaveChangesAsync();
            return true;
        }

        // Remover livro
        public async Task<bool> DeletarAsync(int id)
        {
            var livro = await _context.Livros
                .FirstOrDefaultAsync(l => l.Id == id);

            if (livro == null)
                return false;

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
