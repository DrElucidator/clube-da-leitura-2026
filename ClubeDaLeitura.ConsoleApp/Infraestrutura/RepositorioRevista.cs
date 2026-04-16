using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Apresentacao;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioRevista
{
    private List<Revista> revistas = new List<Revista>();

    public void Cadastrar(Revista revista)
    {
        revistas.Add(revista);
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