
using System.Net;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ClubeDaLeitura.ConsoleApp.Apresentacao;
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

    public void CadastrarRevista(string idCaixa, Revista revista)
    {
        Caixa? caixa = caixas.Find(c => c.Id == idCaixa);
        if (caixa != null)
        {
            caixa.Revistas.Add(revista);
            SalvarNoArquivo();
        }
    }


    public List<Caixa> SelecionarTodas()
    {
        return caixas;
    }

    public void Editar()
    {

    }

    public bool Excluir(string id)
    {
        Caixa excluirCaixa = caixas.Find(cx => cx.Id == id);
        if (excluirCaixa != null && excluirCaixa.Revistas.Count == 0)
        {
            caixas.Remove(excluirCaixa);
            SalvarNoArquivo();
            return true;
        }

        return false;
    }

    public Caixa? BuscarPorId(string id)
    {
        return caixas.Find(cx => cx.Id == id);
    }
}