namespace Biblioteca.Models
{
    public class Pessoa
    {
        public int Id { get; set; } // id unico
        public string Nome { get; set; } = string.Empty; // string.Empty; para evitar null
        public string Cpf { get; set; } = string.Empty;

        // Permite que a Pessoa acesse diretamente seus papéis (Cliente ou Funcionário) no sistema 
        public Cliente? Cliente { get; set; }  
        public Funcionario? Funcionario { get; set; }
  
    }
}
