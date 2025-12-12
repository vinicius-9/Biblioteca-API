using Biblioteca.Data;
using Biblioteca.Dtos;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class ClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClienteResponse> CriarClienteAsync(ClienteRequest request)
        {
            // Validar duplicidade de CPF
            if (await _context.Pessoas.AnyAsync(p => p.Cpf == request.Cpf))
                throw new Exception("CPF já cadastrado.");

            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Cpf = request.Cpf
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync(); // Id gerado

            var cliente = new Cliente
            {
                PessoaId = pessoa.Id, // FK normal
                Ativo = true
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return new ClienteResponse
            {
                Id = cliente.Id,
                Nome = pessoa.Nome,
                Cpf = pessoa.Cpf,
                Ativo = cliente.Ativo
            };
        }

        public async Task<List<ClienteResponse>> ObterTodosAsync()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Pessoa)
                .ToListAsync();

            return clientes.Select(c => new ClienteResponse
            {
                Id = c.Id,
                Nome = c.Pessoa.Nome,
                Cpf = c.Pessoa.Cpf,
                Ativo = c.Ativo
            }).ToList();
        }

        public async Task<ClienteResponse?> ObterPorIdAsync(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Pessoa)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null) return null;

            return new ClienteResponse
            {
                Id = cliente.Id,
                Nome = cliente.Pessoa.Nome,
                Cpf = cliente.Pessoa.Cpf,
                Ativo = cliente.Ativo
            };
        }
    }
}
