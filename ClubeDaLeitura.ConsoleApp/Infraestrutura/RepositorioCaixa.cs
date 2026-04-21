using ClubeDaLeitura.ConsoleApp.Dominio;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioCaixa : RepositorioBase<Caixa>
{
    public RepositorioCaixa() : base("caixas.json") { }

    public override bool Excluir(string id)
    {
        var caixa = BuscarPorId(id);
        if (caixa != null && caixa.Revistas.Count == 0)
            return base.Excluir(id);

        return false;
    }
}