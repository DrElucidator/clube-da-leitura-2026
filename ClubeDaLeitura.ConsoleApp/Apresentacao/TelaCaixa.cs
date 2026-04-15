using System.Security.Cryptography.X509Certificates;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
using ClubeDaLeitura.ConsoleApp.Dominio;
using System.Net;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaCaixa
{
    public RepositorioCaixa repositorioCaixa;
    public TelaCaixa(RepositorioCaixa rC)
    {
        repositorioCaixa = rC;
    }
    public string? ObterOpcaoMenu()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Caixas");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Cadastrar Caixa");
        Console.WriteLine("2 - Editar Caixa");
        Console.WriteLine("3 - Excluir Caixa");
        Console.WriteLine("4 - Visualizar Caixas");
        Console.WriteLine("S - Voltar");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");
        string? opcaoMenuPrincipal = Console.ReadLine()?.ToUpper();

        return opcaoMenuPrincipal;
    }

    public void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Caixa");

        Caixa novaCaixa = ObterDadosCadastrais();

        repositorioCaixa.Cadastrar(novaCaixa);

        Console.WriteLine($"O registro \"{novaCaixa.Etiqueta}\" foi cadastrado.");
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadKey();
    }
    public void Editar()
    {
        
    }

    public void Excluir()
    {

    }

    public void VisualizarTodas(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Vizualização de Caixas");

        Console.WriteLine(
            "{0, 7} | {1, -20} | {2, -10} | {3, -20}",
            "Id", "Etiqueta", "Cor", "Tempo de Empréstimo"
        );

        List<Caixa> caixas = repositorioCaixa.SelecionarTodas();

        if (caixas.Count == 0)
        {
            Console.WriteLine("Nenhuma caixa cadastrada.");
        }
        else
            foreach (Caixa c in caixas)
            {
                Console.WriteLine(
            "{0, 7} | {1, -20} | {2, -10} | {3, -20}",
            c.Id, c.Etiqueta, c.Cor, c.DiasDeEmprestimo
                );
            }
            if (deveExibirCabecalho)
        {
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadKey();
        }
    }
    private Caixa ObterDadosCadastrais()
    {
        Console.WriteLine("Digite a etiqueta da caixa: ");
        string? etiqueta = Console.ReadLine();

        Console.WriteLine("\nSelecione uma das cores");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1 = Vermelho");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("2 = Verde");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("3 = Azul");
        Console.ResetColor();
        Console.WriteLine("4 = Branco\n");

        char codigoCor = Convert.ToChar(Console.ReadLine());
        string cor;

        if (codigoCor == 1)
            cor = "Vermelho";
        else if (codigoCor == 2)
            cor = "Verde";
        else if (codigoCor == 3)
            cor = "Azul";
        else
            cor = "Branco";

        System.Console.WriteLine("informe o tempo de empréstimo das revistas desta caixa: ");
        int diasDeEmprestimo = Convert.ToInt32(Console.ReadLine());

        Caixa novaCaixa = new Caixa(etiqueta, cor, diasDeEmprestimo);

        return novaCaixa;
    }

    private static void ExibirCabecalho(string titulo)
    {
        Console.Clear();
        Console.WriteLine("\nGestão de Caixas\n");
        Console.WriteLine(titulo, "\n");
    }

    private static void ExibirMensagem(string mensagem)
    {
        Console.WriteLine("\n", mensagem, "\n");
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadKey();
    }
}