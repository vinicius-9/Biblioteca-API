using Biblioteca.Dtos;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController] // Indica que esta classe é um controller de API
    [Route("api/auth")] // Rota base: /api/auth
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;

        // Injeção do AuthService
        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        // Endpoint público de login
        [HttpPost("login")] // POST /api/auth/login
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            // Valida credenciais e gera o JWT
            var response = await _authService.LoginAsync(request);

            // Retorna o token para o cliente
            return Ok(response);
        }
    }
}
