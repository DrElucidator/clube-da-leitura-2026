using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioAmigo : RepositorioBase<Amigo>
{
    private readonly RepositorioRevista repositorioRevista;

    public RepositorioAmigo(RepositorioRevista repositorioRevista) : base("amigos.json")
    {
        this.repositorioRevista = repositorioRevista;
    }

    public override bool Excluir(string id)
    {
        var amigo = BuscarPorId(id);
        if (amigo == null) return false;

        foreach (var revista in repositorioRevista.SelecionarTodas())
        {
            if (revista.IdAmigoEmprestado == amigo.Id)
                revista.IdAmigoEmprestado = null;
        }

        bool removido = base.Excluir(id);
        if (removido)
            repositorioRevista.SalvarNoArquivo();

        return removido;
    }
}