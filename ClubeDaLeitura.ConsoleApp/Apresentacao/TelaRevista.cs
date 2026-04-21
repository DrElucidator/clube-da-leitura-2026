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
        var erros = novaRevista.Validar();

        if (erros.Length > 0)
            ExibirErros(erros);
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
        var revistaExistente = repositorioRevista.BuscarPorId(id);

        if (revistaExistente == null)
        {
            ExibirMensagem("Revista não encontrada.");
            return;
        }

        Console.Write("Novo título: ");
        string? novoTitulo = Console.ReadLine();
        string tituloFinal = string.IsNullOrWhiteSpace(novoTitulo) ? revistaExistente.Titulo : novoTitulo;

        Console.Write("Novo número de edição: ");
        string? edicaoStr = Console.ReadLine();
        int edicaoFinal = string.IsNullOrWhiteSpace(edicaoStr) ? revistaExistente.NumeroEdicao :
                          (int.TryParse(edicaoStr, out int edicao) ? edicao : revistaExistente.NumeroEdicao);

        Console.Write("Novo ano de publicação: ");
        string? anoStr = Console.ReadLine();
        int anoFinal = string.IsNullOrWhiteSpace(anoStr) ? revistaExistente.AnoPublicacao :
                       (int.TryParse(anoStr, out int ano) ? ano : revistaExistente.AnoPublicacao);

        Console.WriteLine("Digite o ID da nova caixa (ENTER para manter atual): ");
        string? idCaixa = Console.ReadLine()?.ToUpper();
        string caixaFinal = string.IsNullOrWhiteSpace(idCaixa) ? revistaExistente.IdCaixa : idCaixa;

        Revista revistaAtualizada = new Revista(tituloFinal, edicaoFinal, anoFinal, caixaFinal);

        var erros = revistaAtualizada.Validar();
        if (erros.Length > 0)
            ExibirErros(erros);
        else
        {
            repositorioRevista.Editar(id, revistaAtualizada);
            ExibirMensagem($"Revista \"{id}\" editada com sucesso.");
        }
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Revista", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id da revista para exclusão: ");
        string? id = Console.ReadLine()?.ToUpper();
        var revistaDeletada = repositorioRevista.BuscarPorId(id);

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
                var caixa = repositorioCaixa.BuscarPorId(r.IdCaixa);
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
        string titulo = Console.ReadLine() ?? "";

        Console.Write("Número da edição: ");
        int.TryParse(Console.ReadLine(), out int numeroEdicao);

        Console.Write("Ano de publicação: ");
        int.TryParse(Console.ReadLine(), out int anoPublicacao);

        string idCaixa = SelecionarCaixa();
        if (string.IsNullOrEmpty(idCaixa))
            return new Revista(titulo, numeroEdicao, anoPublicacao, "");

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

        return idSelecionado!;
    }
}