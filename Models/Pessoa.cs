namespace Biblioteca.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        public Cliente? Cliente { get; set; }
    }
}
