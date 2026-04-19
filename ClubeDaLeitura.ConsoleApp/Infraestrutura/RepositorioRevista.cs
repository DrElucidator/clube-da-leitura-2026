using System.Text.Json;
using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

    public class RepositorioRevista
    {
        private List<Revista> revistas = new List<Revista>();
        private readonly string caminhoArquivo = "revistas.json";
        private readonly RepositorioCaixa repositorioCaixa;

        public RepositorioRevista(RepositorioCaixa repositorioCaixa)
        {
            this.repositorioCaixa = repositorioCaixa;
            CarregarDoArquivo();
        }

        private void CarregarDoArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                string json = File.ReadAllText(caminhoArquivo);
                revistas = JsonSerializer.Deserialize<List<Revista>>(json) ?? new List<Revista>();
            }

            foreach (Caixa caixa in repositorioCaixa.SelecionarTodas())
            {
                caixa.Revistas = revistas
                    .Where(r => r.IdCaixa == caixa.Id || caixa.RevistaIds.Contains(r.Id))
                    .ToList();

                caixa.RevistaIds = caixa.Revistas.Select(r => r.Id).ToList();
            }
        }

        private void SalvarNoArquivo()
        {
            string json = JsonSerializer.Serialize(revistas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminhoArquivo, json);
        }

        public void Cadastrar(Revista revista)
        {
            Caixa? caixa = repositorioCaixa.BuscarPorId(revista.IdCaixa);

            if (caixa == null)
                throw new Exception("Caixa não encontrada para associar revista.");

            revistas.Add(revista);

            if (!caixa.RevistaIds.Contains(revista.Id))
                caixa.RevistaIds.Add(revista.Id);

            if (!caixa.Revistas.Any(r => r.Id == revista.Id))
                caixa.Revistas.Add(revista);

            SalvarNoArquivo();
            repositorioCaixa.SalvarNoArquivo();
        }

        public bool Excluir(string id)
        {
            Revista? revista = revistas.Find(r => r.Id == id);
            if (revista != null)
            {
                Caixa? caixa = repositorioCaixa.BuscarPorId(revista.IdCaixa);

                revistas.Remove(revista);

                if (caixa != null)
                {
                    caixa.RevistaIds.Remove(revista.Id);
                    caixa.Revistas.Remove(revista);
                }

                SalvarNoArquivo();
                repositorioCaixa.SalvarNoArquivo();

                return true;
            }

            return false;
        }

        public List<Revista> SelecionarTodas()
        {
            return revistas;
        }

        public Revista? BuscarPorId(string id)
        {
            return revistas.Find(revista => revista.Id == id);
        }
    }