using Biblioteca.Data;
using Biblioteca.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Biblioteca.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Login principal
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // Busca funcionário ativo pelo email
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Email == request.Email && f.Ativo);

            if (funcionario == null)
                throw new Exception("Credenciais inválidas.");

            // Compara hash da senha
            if (funcionario.SenhaHash != GerarHash(request.Senha))
                throw new Exception("Credenciais inválidas.");

            // Gera JWT
            var token = GerarToken(funcionario);

            return new LoginResponse
            {
                Token = token
            };
        }

        // Gera JWT com claims básicas
        private string GerarToken(Models.Funcionario funcionario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, funcionario.Id.ToString()),
                new Claim(ClaimTypes.Email, funcionario.Email),
                new Claim(ClaimTypes.Role, funcionario.Cargo)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Método utilitário para gerar hash da senha
        private static string GerarHash(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }
    }
}
