using Microsoft.AspNetCore.Mvc;
using Biblioteca.Services;
using Biblioteca.Dtos;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponse>> Criar(ClienteRequest request)
        {
            var cliente = await _clienteService.CriarClienteAsync(request);
            return Ok(cliente);
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteResponse>>> ObterTodos()
        {
            return Ok(await _clienteService.ObterTodosAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponse>> ObterPorId(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        // ------------------- NOVO: Atualizar Cliente -------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteResponse>> Atualizar(int id, ClienteRequest request)
        {
            var clienteAtualizado = await _clienteService.AtualizarClienteAsync(id, request);

            if (clienteAtualizado == null)
                return NotFound();

            return Ok(clienteAtualizado);
        }
    }
}
