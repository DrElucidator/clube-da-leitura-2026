using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaRevista : TelaBase<Revista>
{
    public override string NomeGestao => "Revistas";
    private readonly RepositorioRevista repositorioRevista;
    private readonly RepositorioCaixa repositorioCaixa;

    public TelaRevista(RepositorioRevista rR, RepositorioCaixa rC)
    {
        repositorioRevista = rR;
        repositorioCaixa = rC;
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Revista", NomeGestao);
        Revista novaRevista = ObterDadosCadastrais();
        if (novaRevista == null)
            ExibirMensagem("Cadastro cancelado.");
        else
        {
            repositorioRevista.Cadastrar(novaRevista);
            ExibirMensagem($"O registro \"{novaRevista.Titulo}\" foi cadastrado.");
        }
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Revista", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id da revista para edição: ");
        string? id = Console.ReadLine()?.ToUpper();
        Revista? revistaEditada = repositorioRevista.BuscarPorId(id);

        if (revistaEditada == null)
            ExibirMensagem("Revista não encontrada.");
        else
        {
            Console.Write("Novo título: ");
            string? novoTitulo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoTitulo))
                revistaEditada.Titulo = novoTitulo;

            Console.Write("Novo número de edição: ");
            if (int.TryParse(Console.ReadLine(), out int novaEdicao))
                revistaEditada.NumeroEdicao = novaEdicao;

            Console.Write("Novo ano de publicação: ");
            if (int.TryParse(Console.ReadLine(), out int novoAno))
                revistaEditada.AnoPublicacao = novoAno;

            ExibirMensagem($"Revista \"{revistaEditada.Id}\" editada com sucesso.");
        }
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Revista", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id da revista para exclusão: ");
        string? id = Console.ReadLine()?.ToUpper();
        Revista? revistaDeletada = repositorioRevista.BuscarPorId(id);

        if (revistaDeletada == null)
            ExibirMensagem("Revista não encontrada.");
        else if (repositorioRevista.Excluir(id))
            ExibirMensagem($"O registro \"{revistaDeletada.Titulo}\" foi excluído.");
        else
            ExibirMensagem("Falha na exclusão.");
    }

    public override void VisualizarTodas(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Revistas", NomeGestao);

        Console.WriteLine("{0, -7} | {1, -25} | {2, -6} | {3, -4} | {4, -15}",
            "Id", "Título", "Edição", "Ano", "Caixa");

        var revistas = repositorioRevista.SelecionarTodas();
        if (revistas.Count == 0)
            Console.WriteLine("Nenhuma revista cadastrada.");
        else
            foreach (var r in revistas)
            {
                Caixa? caixa = repositorioCaixa.BuscarPorId(r.IdCaixa);
                string etiquetaCaixa = caixa != null ? caixa.Etiqueta : "Caixa não encontrada";

                if (caixa?.Cor == "Vermelho") Console.ForegroundColor = ConsoleColor.Red;
                else if (caixa?.Cor == "Verde") Console.ForegroundColor = ConsoleColor.Green;
                else if (caixa?.Cor == "Azul") Console.ForegroundColor = ConsoleColor.Blue;
                else Console.ResetColor();

                Console.WriteLine("{0, -7} | {1, -25} | {2, -6} | {3, -4} | {4, -15}",
                    r.Id, r.Titulo, r.NumeroEdicao, r.AnoPublicacao, etiquetaCaixa);

                Console.ResetColor();
            }

        if (deveExibirCabecalho) ExibirMensagem("");
    }

    private Revista ObterDadosCadastrais()
    {
        Console.Write("Título: ");
        string titulo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(titulo))
        {
            Console.WriteLine("Título inválido.");
            return null;
        }

        Console.Write("Número da edição: ");
        int.TryParse(Console.ReadLine(), out int numeroEdicao);

        Console.Write("Ano de publicação: ");
        int.TryParse(Console.ReadLine(), out int anoPublicacao);

        string idCaixa = SelecionarCaixa();
        if (string.IsNullOrEmpty(idCaixa))
            return null;

        return new Revista(titulo, numeroEdicao, anoPublicacao, idCaixa);
    }

    private string SelecionarCaixa()
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine("{0, -7} | {1, -20} | {2, -10} | {3, -20}",
            "Id", "Etiqueta", "Cor", "Dias");

        var caixas = repositorioCaixa.SelecionarTodas();
        if (caixas.Count == 0)
        {
            Console.WriteLine("Nenhuma caixa cadastrada.");
            return string.Empty;
        }

        foreach (var cx in caixas)
        {
            if (cx.Cor == "Vermelho") Console.ForegroundColor = ConsoleColor.Red;
            else if (cx.Cor == "Verde") Console.ForegroundColor = ConsoleColor.Green;
            else if (cx.Cor == "Azul") Console.ForegroundColor = ConsoleColor.Blue;
            else Console.ResetColor();

            Console.WriteLine("{0, -7} | {1, -20} | {2, -10} | {3, -20}",
                cx.Id, cx.Etiqueta, cx.Cor, cx.DiasDeEmprestimo);

            Console.ResetColor();
        }

        string? idSelecionado;
        do
        {
            Console.Write("Digite o ID da caixa: ");
            idSelecionado = Console.ReadLine()?.ToUpper();

            if (!string.IsNullOrWhiteSpace(idSelecionado) &&
                repositorioCaixa.BuscarPorId(idSelecionado) != null)
                break;

            Console.WriteLine("ID inválido, tente novamente.");
        } while (true);

        return idSelecionado;
    }
}