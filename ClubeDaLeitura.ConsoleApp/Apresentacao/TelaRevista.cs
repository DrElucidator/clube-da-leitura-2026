using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaRevista
{
    private RepositorioRevista repositorioRevista;
    private RepositorioCaixa repositorioCaixa;

    public TelaRevista(RepositorioRevista rR, RepositorioCaixa rC)
    {
        repositorioRevista = rR;
        repositorioCaixa = rC;
    }

    public string? ObterOpcaoMenu()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Revistas");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Cadastrar Revista");
        Console.WriteLine("2 - Editar Revista");
        Console.WriteLine("3 - Excluir Revista");
        Console.WriteLine("4 - Visualizar Revista");
        Console.WriteLine("S - Voltar");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");
        string? opcaoMenu = Console.ReadLine()?.ToUpper();

        return opcaoMenu;
    }

    public void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Revistas");

        Revista novaRevista = ObterDadosCadastrais();

        if (novaRevista == null)
        {
            ExibirMensagem("Cadastro cancelado.");
        }
        else
        {
            repositorioRevista.Cadastrar(novaRevista);
            ExibirMensagem($"O registro \"{novaRevista.Titulo}\" foi cadastrado.");
        }
    }

    public void Editar()
    {

    }

    public void Excluir()
    {
        ExibirCabecalho("Exclusão de Revista");

        Console.WriteLine(
            "{0, -7} | {1, -25} | {2, -6} | {3, -4} | {4, -15}",
            "Id", "Título", "Edição", "Ano", "Caixa"
        );

        List<Revista> revistas = repositorioRevista.SelecionarTodas();

        if (revistas.Count == 0)
        {
            Console.WriteLine("Nenhuma revista cadastrada.");
        }
        else
        {
            foreach (Revista revst in revistas)
            {
                Console.Write("{0, -7} | ", revst.Id);
                Console.Write("{0, -25} | ", revst.Titulo);
                Console.Write("{0, -6} | ", revst.NumeroEdicao);
                Console.Write("{0, -4} | ", revst.AnoPublicacao);

                string corSelecionada = revst.Caixa.Cor;

                if (corSelecionada == "Vermelho")
                    Console.ForegroundColor = ConsoleColor.Red;

                else if (corSelecionada == "Verde")
                    Console.ForegroundColor = ConsoleColor.Green;

                else if (corSelecionada == "Azul")
                    Console.ForegroundColor = ConsoleColor.Blue;

                Console.Write("{0, -15}", revst.Caixa.Etiqueta);

                Console.ResetColor();
            }
            Console.WriteLine("\nDigite o id da revista para exclusão: ");
            string? id = Console.ReadLine().ToUpper();

            Revista? revistaDeletada = repositorioRevista.BuscarPorId(id);

            if (revistaDeletada == null)
                Console.WriteLine("Revista não encontrada.");
            else
            {
                repositorioCaixa.Excluir(id);
                ExibirMensagem($"O registro \"{revistaDeletada.Titulo}\" foi excluido.");
            }
        }
    }

    public void VisualizarTodas(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Vizualização de Revistas");

        Console.WriteLine(
            "{0, -7} | {1, -25} | {2, -6} | {3, -4} | {4, -15}",
            "Id", "Título", "Edição", "Ano", "Caixa"
        );

        List<Revista> revistas = repositorioRevista.SelecionarTodas();

        if (revistas.Count == 0)
        {
            Console.WriteLine("\nNenhuma revista cadastrada.");
        }
        else
            foreach (Revista revst in revistas)
            {
                Console.Write("{0, -7} | ", revst.Id);
                Console.Write("{0, -25} | ", revst.Titulo);
                Console.Write("{0, -6} | ", revst.NumeroEdicao);
                Console.Write("{0, -4} | ", revst.AnoPublicacao);

                string corSelecionada = revst.Caixa.Cor;

                if (corSelecionada == "Vermelho")
                    Console.ForegroundColor = ConsoleColor.Red;

                else if (corSelecionada == "Verde")
                    Console.ForegroundColor = ConsoleColor.Green;

                else if (corSelecionada == "Azul")
                    Console.ForegroundColor = ConsoleColor.Blue;

                Console.Write("{0, -15}", revst.Caixa.Etiqueta);

                Console.ResetColor();
                Console.WriteLine();
            }

        if (deveExibirCabecalho)
        {
            ExibirMensagem("");
        }
    }

    private Revista ObterDadosCadastrais()
    {
        ExibirCabecalho("Digite o título da revista: ");
        string titulo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(titulo) || titulo.Length > 100)
        {
            Console.WriteLine("Título da revista inválido, digite um título válido de até 50 caracteres.");
            return null;
        }

        Console.WriteLine("Digite o número da edição: ");
        int numeroEdicao = Convert.ToInt32(Console.ReadLine());

        Console.Write("Digite o ano de publicação: ");
        int anoPublicacao = Convert.ToInt32(Console.ReadLine());

        string idSelecionado = SelecionarCaixa();

        Caixa? caixaSelecionada = repositorioCaixa.BuscarPorId(idSelecionado);

        return new Revista(titulo, numeroEdicao, anoPublicacao, caixaSelecionada);
    }

    private string SelecionarCaixa()
    {
        Console.WriteLine("---------------------------------");

        Console.WriteLine(
          "{0, -7} | {1, -20} | {2, -10} | {3, -20}",
          "Id", "Etiqueta", "Cor", "Tempo de Empréstimo"
      );

        List<Caixa> caixas = repositorioCaixa.SelecionarTodas();

        if (caixas.Count == 0)
        {
            Console.WriteLine("\nNenhuma caixa cadastrada.");
        }
        else
            foreach (Caixa cx in caixas)
            {
                string corSelecionada = cx.Cor;

                if (corSelecionada == "Vermelho")
                    Console.ForegroundColor = ConsoleColor.Red;

                else if (corSelecionada == "Verde")
                    Console.ForegroundColor = ConsoleColor.Green;

                else if (corSelecionada == "Azul")
                    Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine(
            "{0, 7} | {1, -20} | {2, -10} | {3, -20}",
            cx.Id, cx.Etiqueta, cx.Cor, cx.DiasDeEmprestimo
                );
            }
        Console.ResetColor();

        string? idSelecionado;

        do
        {
            Console.Write("Digite o ID da caixa em que deseja guardar a revista: ");
            idSelecionado = Console.ReadLine().ToUpper();

            if (!string.IsNullOrWhiteSpace(idSelecionado) && idSelecionado.Length == 5)
                break;
        } while (true);

        return idSelecionado;
    }

    private static void ExibirCabecalho(string titulo)
    {
        Console.Clear();
        Console.WriteLine("\nGestão de Revistas\n");
        Console.WriteLine(titulo, "\n");
    }

    private static void ExibirMensagem(string mensagem)
    {
        Console.WriteLine("\n", mensagem, "\n");
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadKey();
    }
}