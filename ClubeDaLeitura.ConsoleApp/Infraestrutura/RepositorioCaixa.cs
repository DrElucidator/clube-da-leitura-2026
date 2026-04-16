
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioCaixa
{
    private List<Caixa> caixas = new List<Caixa>();

    public void Cadastrar(Caixa caixa)
    {
        caixas.Add(caixa);
    }

    public List<Caixa> SelecionarTodas()
    {
        return caixas;
    }
    public void Editar()
    {

    }

    public void Excluir(string id)
    {
        Caixa excluirCaixa = caixas.Find(cx => cx.Id == id);
        if (excluirCaixa != null)
            caixas.Remove(excluirCaixa); 
    }
    public Caixa? BuscarPorId(string id)
    {
        return caixas.Find(cx => cx.Id == id);
    }
}