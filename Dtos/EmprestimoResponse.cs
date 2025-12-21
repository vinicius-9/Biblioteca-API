namespace Biblioteca.Dtos
{
   
    public class EmprestimoResponse
    {
        public int Id { get; set; } // Id do empréstimo

        // Dados do livro 
        public string LivroTitulo { get; set; } = string.Empty;
        public int LivroAno { get; set; }

        // Dados do funcionário
        public string FuncionarioNome { get; set; } = string.Empty;
        public string FuncionarioCargo { get; set; } = string.Empty;

        // Datas do empréstimo
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
    }
}
