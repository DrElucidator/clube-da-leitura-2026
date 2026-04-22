using System.Text.Json;
using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

    public class RepositorioEmprestimo
    {
        private List<Emprestimo> emprestimos = new List<Emprestimo>();
        private readonly string caminhoArquivo = "emprestimos.json";

        public RepositorioEmprestimo()
        {
            CarregarDoArquivo();
        }

        private void CarregarDoArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                string json = File.ReadAllText(caminhoArquivo);
                emprestimos = JsonSerializer.Deserialize<List<Emprestimo>>(json) ?? new List<Emprestimo>();
            }
        }

        private void SalvarNoArquivo()
        {
            string json = JsonSerializer.Serialize(emprestimos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminhoArquivo, json);
        }

        public void Cadastrar(Emprestimo emprestimo)
        {
            emprestimos.Add(emprestimo);
            SalvarNoArquivo();
        }

        public bool Excluir(string id)
        {
            Emprestimo? emprestimo = BuscarPorId(id);
            if (emprestimo != null)
            {
                emprestimos.Remove(emprestimo);
                SalvarNoArquivo();
                return true;
            }
            return false;
        }

        public bool Atualizar(Emprestimo emprestimo)
        {
            var existente = BuscarPorId(emprestimo.Id);
            if (existente != null)
            {
                existente.AtualizarRegistro(emprestimo);
                SalvarNoArquivo();
                return true;
            }
            return false;
        }

        public List<Emprestimo> SelecionarTodos()
        {
            return emprestimos;
        }

        public Emprestimo? BuscarPorId(string id)
        {
            return emprestimos.Find(e => e.Id == id);
        }
    }