namespace Biblioteca.Dtos
{
    // DTO que representa a resposta do servidor após login
    public class LoginResponse
    {
        // Token JWT gerado após login válido
        // Serve para autenticação das próximas requisições
        public string Token { get; set; } = string.Empty;
    }
}
