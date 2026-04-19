using System.Text.Json;
using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Apresentacao;
using System.Runtime.CompilerServices;
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
                .Where(r => caixa.RevistaIds.Contains(r.Id))
                .ToList();
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

        revistas.Add(revista);
        caixa.RevistaIds.Add(revista.Id);
        caixa.Revistas.Add(revista);

        SalvarNoArquivo();
        repositorioCaixa.SalvarNoArquivo();
    }

    public bool Excluir(string id)
    {
        Revista revista = revistas.Find(r => r.Id == id);
        if (revista != null)
        {
            Caixa? caixa = repositorioCaixa.BuscarPorId(revista.IdCaixa);
            revistas.Remove(revista);

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