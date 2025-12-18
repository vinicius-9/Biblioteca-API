namespace Biblioteca.Models
{
    public class Livro
    {
        public int Id  { get; set; } //Indentificador
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public bool Ativo { get; set; } = true;         
        
    }

}