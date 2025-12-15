namespace Biblioteca.Dtos;

public class AtualizarSenhaRequest
{
    // Senha atual do funcionário. Será usada para verificar se a pessoa tem autorização para alterar a senha.
    public string SenhaAntiga { get; set; } = string.Empty;

    // Nova senha que o funcionário quer definir. Será transformada em hash antes de salvar no banco.
    public string NovaSenha { get; set; } = string.Empty;
}
