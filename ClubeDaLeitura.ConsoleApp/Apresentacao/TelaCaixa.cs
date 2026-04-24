using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaCaixa : TelaBase
{
    public override string NomeGestao => "Caixas";
    private readonly RepositorioCaixa repositorioCaixa;

    public TelaCaixa(RepositorioCaixa rC)
    {
        repositorioCaixa = rC;
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Caixa", NomeGestao);
        Caixa novaCaixa = ObterDadosCadastrais();
        var erros = novaCaixa.Validar();

        if (erros.Length > 0)
            ExibirErros(erros);
        else
        {
            repositorioCaixa.Cadastrar(novaCaixa);
            ExibirMensagem($"O registro \"{novaCaixa.Etiqueta}\" foi cadastrado.");
        }
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Caixa", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id da caixa para edição: ");
        string? id = Console.ReadLine()?.ToUpper();
        var caixaExistente = repositorioCaixa.BuscarPorId(id);

        if (caixaExistente == null)
        {
            ExibirMensagem("Caixa não encontrada.");
            return;
        }

        Console.Write("Nova etiqueta: ");
        string? novaEtiqueta = Console.ReadLine();
        string etiquetaFinal = string.IsNullOrWhiteSpace(novaEtiqueta) ? caixaExistente.Etiqueta : novaEtiqueta;

        Console.Write("Novo tempo de empréstimo: ");
        string? diasStr = Console.ReadLine();
        int diasFinal = string.IsNullOrWhiteSpace(diasStr) ? caixaExistente.DiasDeEmprestimo :
                        (int.TryParse(diasStr, out int dias) ? dias : caixaExistente.DiasDeEmprestimo);

        Console.WriteLine("Escolha uma cor:");
        Console.WriteLine("1 = Vermelho, 2 = Verde, 3 = Azul, 4 = Branco");
        string? codigoCor = Console.ReadLine();
        string corFinal = string.IsNullOrWhiteSpace(codigoCor) ? caixaExistente.Cor : codigoCor switch
        {
            "1" => "Vermelho",
            "2" => "Verde",
            "3" => "Azul",
            _ => "Branco"
        };

        Caixa caixaAtualizada = new Caixa(etiquetaFinal, corFinal, diasFinal);

        var erros = caixaAtualizada.Validar();
        if (erros.Length > 0)
            ExibirErros(erros);
        else
        {
            repositorioCaixa.Editar(id, caixaAtualizada);
            ExibirMensagem($"Caixa \"{id}\" editada com sucesso.");
        }
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Caixa", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id da caixa para exclusão: ");
        string? id = Console.ReadLine()?.ToUpper();
        var caixaDeletada = repositorioCaixa.BuscarPorId(id);

        if (caixaDeletada == null)
            ExibirMensagem("Caixa não encontrada.");
        else if (repositorioCaixa.Excluir(id))
            ExibirMensagem($"O registro \"{caixaDeletada.Etiqueta}\" foi excluído.");
        else
            ExibirMensagem("Falha ao excluir: Caixa contém revistas.");
    }

    public override void VisualizarTodas(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Caixas", NomeGestao);

        Console.WriteLine("{0, -7} | {1, -20} | {2, -10} | {3, -20}", "Id", "Etiqueta", "Cor", "Dias");
        var caixas = repositorioCaixa.SelecionarTodas();

        if (caixas.Count == 0)
            Console.WriteLine("Nenhuma caixa cadastrada.");
        else
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

        if (deveExibirCabecalho) ExibirMensagem("");
    }

    private Caixa ObterDadosCadastrais()
    {
        Console.Write("Etiqueta: ");
        string etiqueta = Console.ReadLine() ?? "";

        Console.WriteLine("Escolha uma cor:");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1 = Vermelho");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("2 = Verde");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("3 = Azul");
        Console.ResetColor();
        Console.WriteLine("4 = Branco");

        string? codigoCor = Console.ReadLine();
        string cor = codigoCor switch
        {
            "1" => "Vermelho",
            "2" => "Verde",
            "3" => "Azul",
            _ => "Branco"
        };

        Console.Write("Dias de empréstimo: ");
        int.TryParse(Console.ReadLine(), out int dias);
        if (dias <= 0) dias = 7;

        return new Caixa(etiqueta, cor, dias);
    }
}