namespace Biblioteca.Models
{
    public class Cliente
    {
        public int Id { get; set; }        // PK independente
        public int PessoaId { get; set; }  // Ela aponta para Pessoa

        public Pessoa Pessoa { get; set; } = null!; // acessa todos os dados da pessoa
        public bool Ativo { get; set; } = true;
    }
}
