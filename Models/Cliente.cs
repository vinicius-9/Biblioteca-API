namespace Biblioteca.Models
{
    public class Cliente
    {
        public int Id { get; set; }        // PK independente
        public int PessoaId { get; set; }  // FK para Pessoa

        public Pessoa Pessoa { get; set; } = null!;
        public bool Ativo { get; set; } = true;
    }
}
