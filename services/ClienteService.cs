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
            // Guarda o contexto do EF, que permite acessar o banco de dados
            _context = context;
        }

        public async Task<ClienteResponse> CriarClienteAsync(ClienteRequest request)
        {
            // Antes de criar alguém, verificamos se já existe uma Pessoa com o mesmo CPF.
            if (await _context.Pessoas.AnyAsync(p => p.Cpf == request.Cpf))
                throw new Exception("CPF já cadastrado.");

            // Criamos primeiro a entidade Pessoa — porque todo Cliente obrigatoriamente precisa de uma Pessoa associada.
            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Cpf = request.Cpf
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            // Criamos o Cliente ligado a essa Pessoa.
            var cliente = new Cliente
            {
                PessoaId = pessoa.Id,
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

        // ------------------- NOVO: Atualizar Cliente -------------------
        public async Task<ClienteResponse?> AtualizarClienteAsync(int id, ClienteRequest request)
        {
            // Busca o cliente pelo Id, incluindo a Pessoa associada.
            var cliente = await _context.Clientes
                .Include(c => c.Pessoa)
                .FirstOrDefaultAsync(c => c.Id == id);

            // Se não existir, retorna null
            if (cliente == null)
                return null;

            // Atualiza apenas o Nome da Pessoa.
            // CPF é ignorado para não quebrar regra de negócio.
            cliente.Pessoa.Nome = request.Nome;

            // Salva as alterações no banco.
            await _context.SaveChangesAsync();

            // Retorna o cliente atualizado.
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
