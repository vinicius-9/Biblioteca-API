using Biblioteca.Dtos;      
using Biblioteca.Services;  
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController] 
    [Route("api/funcionarios")] 
    public class FuncionarioController : ControllerBase
    {
        // Instância do service de funcionário, usado para acessar a lógica de negócio
        private readonly FuncionarioService _funcionarioService;

        // Construtor recebe o service via injeção de dependência
        public FuncionarioController(FuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        // Cria um novo funcionário
        [HttpPost] 
        public async Task<ActionResult<FuncionarioResponse>> Criar(FuncionarioRequest request)
        {
            var response = await _funcionarioService.CriarFuncionarioAsync(request);
            return Ok(response);
        }

        // Lista todos os funcionários
        [HttpGet] 
        public async Task<ActionResult<List<FuncionarioResponse>>> ObterTodos()
        {
            var response = await _funcionarioService.ObterTodosAsync();
            return Ok(response);
        }

        // Busca funcionário por Id
        [HttpGet("{id}")] 
        public async Task<ActionResult<FuncionarioResponse>> ObterPorId(int id)
        {
            var response = await _funcionarioService.ObterPorIdAsync(id);
            if (response == null)
                return NotFound();
            return Ok(response);
        }

        // Atualiza funcionário existente (exceto CPF)
        [HttpPut("{id}")] 
        public async Task<ActionResult<FuncionarioResponse>> Atualizar(int id, FuncionarioRequest request)
        {
            try
            {
                var response = await _funcionarioService.AtualizarFuncionarioAsync(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Novo endpoint: Atualiza apenas a senha do funcionário
        [HttpPut("{id}/senha")]
        public async Task<ActionResult> AtualizarSenha(int id, AtualizarSenhaRequest request)
        {
            try
            {
                // Chama o service para alterar a senha com verificação da senha antiga
                await _funcionarioService.AtualizarSenhaAsync(id, request);

                // Retorna mensagem de sucesso
                return Ok(new { mensagem = "Senha atualizada com sucesso." });
            }
            catch (Exception ex)
            {
                // Retorna erro caso senha antiga esteja incorreta ou outro problema
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
