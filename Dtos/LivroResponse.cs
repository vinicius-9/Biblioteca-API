namespace Biblioteca.Dtos
{
    public class LivroResponse
    {  
        public int Id { get; set; }
        public string Titulo { get; set; }  = string.Empty;
        public string Autor { get; set; }  = string.Empty;
        public int AnoPublicacao { get; set; }
        public bool Ativo { get; set; }      
        
    }
}