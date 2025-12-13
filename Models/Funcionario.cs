namespace Biblioteca.Models
{
    public class Funcionario
    {
      public int Id { get; set; }  //Indentificador do funcionario
      public int PessoaId { get; set; } // chave estrangeira aponta pra pessoa 

      public Pessoa Pessoa { get; set; } = null!;  //navegaçao acessa pessoa e pega  seus dados
        

      public string Cargo { get; set; } = string.Empty;
     
      public string Email { get; set; } = string.Empty;
      public string SenhaHash { get; set; } = string.Empty;  // senha protegida
      
      public bool Ativo { get; set; } = true;

    }
}