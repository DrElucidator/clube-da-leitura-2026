using System.Text.Json;
using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

    public class RepositorioCaixa
    {
        private List<Caixa> caixas = new List<Caixa>();
        private readonly string caminhoArquivo = "caixas.json";

        public RepositorioCaixa()
        {
            CarregarDoArquivo();
        }

        private void CarregarDoArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                string json = File.ReadAllText(caminhoArquivo);
                caixas = JsonSerializer.Deserialize<List<Caixa>>(json) ?? new List<Caixa>();
            }
        }

        public void SalvarNoArquivo()
        {
            string json = JsonSerializer.Serialize(caixas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminhoArquivo, json);
        }

        public void Cadastrar(Caixa caixa)
        {
            caixas.Add(caixa);
            SalvarNoArquivo();
        }

        public bool Excluir(string id)
        {
            Caixa? caixa = BuscarPorId(id);
            if (caixa != null && caixa.Revistas.Count == 0)
            {
                caixas.Remove(caixa);
                SalvarNoArquivo();
                return true;
            }
            return false;
        }

        public List<Caixa> SelecionarTodas()
        {
            return caixas;
        }

        public Caixa? BuscarPorId(string id)
        {
            return caixas.Find(c => c.Id == id);
        }
    }