using Biblioteca.Dtos;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly LivroService _service;

        public LivroController(LivroService service)
        {
            _service = service;
        }

        // Método para criar um novo livro
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] LivroRequest request)
        {
            var livro = await _service.CriarAsync(request);

            return CreatedAtAction(
                nameof(ObterPorId), 
                new { id = livro.Id }, 
                livro
            );
        }

        // Método para obter um livro pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var livro = await _service.ObterPorIdAsync(id);

            if (livro == null)
                return NotFound();

            return Ok(livro);
        }

        // Método para atualizar um livro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] LivroRequest request)
        {
            var atualizado = await _service.AtualizarAsync(id, request);

            if (!atualizado)
                return NotFound();

            return NoContent();
        }

        // Método para remover um livro do banco (delete físico)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var removido = await _service.DeletarAsync(id);

            if (!removido)
                return NotFound();

            return NoContent();
        }

        // Novo método: ativar/desativar livro
        [HttpPatch("{id}/ativo")]
        public async Task<IActionResult> AtualizarAtivo(int id, [FromBody] bool ativo)
        {
            var atualizado = await _service.AtualizarAtivoAsync(id, ativo);

            if (!atualizado)
                return NotFound();

            return NoContent();
        }
    }
}
