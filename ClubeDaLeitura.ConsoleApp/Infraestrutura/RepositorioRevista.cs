using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioRevista : RepositorioBase<Revista>
{
    private readonly RepositorioCaixa repositorioCaixa;

    public RepositorioRevista(RepositorioCaixa repositorioCaixa) : base("revistas.json")
    {
        this.repositorioCaixa = repositorioCaixa;
        SincronizarCaixas();
    }

    private void SincronizarCaixas()
    {
        foreach (var caixa in repositorioCaixa.SelecionarTodas())
        {
            caixa.Revistas = SelecionarTodas()
                .Where(r => r.IdCaixa == caixa.Id || caixa.RevistaIds.Contains(r.Id))
                .ToList();

            caixa.RevistaIds = caixa.Revistas.Select(r => r.Id).ToList();
        }
    }

    public new void Cadastrar(Revista revista)
    {
        var caixa = repositorioCaixa.BuscarPorId(revista.IdCaixa);
        if (caixa == null)
            throw new Exception("Caixa não encontrada para associar revista.");

        base.Cadastrar(revista);

        if (!caixa.RevistaIds.Contains(revista.Id))
            caixa.RevistaIds.Add(revista.Id);

        if (!caixa.Revistas.Any(r => r.Id == revista.Id))
            caixa.Revistas.Add(revista);

        repositorioCaixa.SalvarNoArquivo();
    }

    public override bool Excluir(string id)
    {
        var revista = BuscarPorId(id);
        if (revista != null)
        {
            var caixa = repositorioCaixa.BuscarPorId(revista.IdCaixa);

            bool removido = base.Excluir(id);

            if (removido && caixa != null)
            {
                caixa.RevistaIds.Remove(revista.Id);
                caixa.Revistas.Remove(revista);
                repositorioCaixa.SalvarNoArquivo();
            }

            return removido;
        }
        return false;
    }
}