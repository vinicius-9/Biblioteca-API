namespace Biblioteca.Dtos
{
    // DTO que representa os dados enviados pelo cliente ao fazer login
    public class LoginRequest
    {
        // Email usado como login
        public string Email { get; set; } = string.Empty;

        // Senha do funcionário (em texto apenas aqui, será verificada via hash)
        public string Senha { get; set; } = string.Empty;
    }
}
