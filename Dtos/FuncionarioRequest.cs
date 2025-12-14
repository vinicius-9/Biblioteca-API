namespace Biblioteca.Dtos
{
    public class FuncionarioRequest
    { 
        public string Nome  { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;

    }
}