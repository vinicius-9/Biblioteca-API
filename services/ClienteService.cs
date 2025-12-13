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

            // Inserimos essa pessoa no banco...
            _context.Pessoas.Add(pessoa);

            // ...e salvamos para que o banco gere o Id (chave primária).
            await _context.SaveChangesAsync();

            // Agora que a pessoa existe e já tem um Id,
            // criamos o Cliente ligado a essa Pessoa.
            var cliente = new Cliente
            {
                // A FK apontando para Pessoa.Id garante a relação 1-para-1.
                PessoaId = pessoa.Id,
                Ativo = true
            };

            // Inserimos o Cliente no banco
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Retornamos um DTO (ClienteResponse) contendo informações tanto da Pessoa quanto do Cliente.
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
            // Pegamos todos os clientes do banco, mas com um detalhe importante:
            // Include(c => c.Pessoa) faz o EF também carregar os dados da Pessoa associada a cada Cliente.
            var clientes = await _context.Clientes
                .Include(c => c.Pessoa)
                .ToListAsync();

            // Convertendo cada entidade Cliente em ClienteResponse (DTO).
            return clientes.Select(c => new ClienteResponse
            {
                Id = c.Id,
                Nome = c.Pessoa.Nome, // Nome vem da Pessoa associada
                Cpf = c.Pessoa.Cpf,   // CPF também
                Ativo = c.Ativo
            }).ToList();
        }

        public async Task<ClienteResponse?> ObterPorIdAsync(int id)
        {
            // Busca um único Cliente pelo Id.
        
            var cliente = await _context.Clientes
                .Include(c => c.Pessoa)
                .FirstOrDefaultAsync(c => c.Id == id);

            // Se não achou o cliente, devolve null.
            if (cliente == null) return null;

            // Monta o DTO com as informações combinadas.
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
