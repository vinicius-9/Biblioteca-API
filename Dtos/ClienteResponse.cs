namespace Biblioteca.Dtos;

public class ClienteResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
