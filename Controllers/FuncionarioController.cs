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
            // Chama o service que busca todos os funcionários
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


        // Atualiza funcionário existente
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
    }
}
