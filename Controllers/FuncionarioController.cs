using Biblioteca.Dtos;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    // [Authorize] <- removido para permitir criar funcionários sem login
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioService _funcionarioService;

        public FuncionarioController(FuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        [HttpPost]
        public async Task<ActionResult<FuncionarioResponse>> Criar(FuncionarioRequest request)
        {
            var response = await _funcionarioService.CriarFuncionarioAsync(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<FuncionarioResponse>>> ObterTodos()
        {
            var response = await _funcionarioService.ObterTodosAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioResponse>> ObterPorId(int id)
        {
            var response = await _funcionarioService.ObterPorIdAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}
