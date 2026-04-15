
using System.Net;
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

    public void Editar()
    {

    }

    public void Excluir()
    {
        
    }

    public void VisualizarTodas()
    {
        
    }
}