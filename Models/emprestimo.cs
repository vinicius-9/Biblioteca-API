namespace Biblioteca.Models
{
    
    public class Emprestimo
    {
        public int Id { get; set; }

        // Livro relacionado ao empréstimo (FK no banco)
        public int LivroId { get; set; }

        // Navegação: permite acessar os dados do livro a partir do empréstimo
        public Livro Livro { get; set; } = null!;

        // Cliente relacionado ao empréstimo
        public int ClienteId { get; set; }

        // Navegação: permite acessar quem realizou o empréstimo
        public Cliente Cliente { get; set; } = null!;

        // Funcionário responsável pelo registro
        public int FuncionarioId { get; set; }

        // Navegação: permite acessar quem registrou o empréstimo
        public Funcionario Funcionario { get; set; } = null!;

        // Datas do processo
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; } // null = ainda não devolvido
    }
}
