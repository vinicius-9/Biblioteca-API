using Biblioteca.Dtos;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("api/emprestimos")]
    public class EmprestimoController : ControllerBase
    {
        private readonly EmprestimoService _emprestimoService;

        public EmprestimoController(EmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }

        // Cria novo emprestimo
        [HttpPost]
        public async Task<ActionResult<EmprestimoResponse>> Criar(EmprestimoRequest request)
        {
            try
            {
                var response = await _emprestimoService.CriarAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // Busca emprestimo por id
        [HttpGet("{id}")]
        public async Task<ActionResult<EmprestimoResponse>> ObterPorId(int id)
        {
            var response = await _emprestimoService.ObterPorIdAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        // Lista todos emprestimos
        [HttpGet]
        public async Task<ActionResult<List<EmprestimoResponse>>> ObterTodos()
        {
            var response = await _emprestimoService.ObterTodosAsync();
            return Ok(response);
        }

        // Atualizar DataDevolucao
        [HttpPatch("{id}/devolucao")]
        public async Task<IActionResult> AtualizarDataDevolucao(
            int id,
            [FromBody] DateTime dataDevolucao)
        {
            var atualizado = await _emprestimoService.AtualizarDataDevolucaoAsync(id, dataDevolucao);

            if (!atualizado)
                return NotFound();

            return NoContent();
        }
    }
}
