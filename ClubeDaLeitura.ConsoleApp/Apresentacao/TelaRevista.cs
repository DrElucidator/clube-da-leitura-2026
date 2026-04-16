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
        Console.WriteLine("1 - Cadastrar Caixa");
        Console.WriteLine("2 - Editar Caixa");
        Console.WriteLine("3 - Excluir Caixa");
        Console.WriteLine("4 - Visualizar Caixas");
        Console.WriteLine("S - Voltar");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");
        string? opcaoMenu = Console.ReadLine()?.ToUpper();

        return opcaoMenu;
    }

    public void Cadastrar()
    {
        
    }

    public void Editar()
    {
        
    }

    public void Excluir()
    {
        
    }

    public void VizualizarTodas()
    {
        
    }

    private Revista ObterDadosCadastrais()
    {
        string titulo = ""; int numeroEdicao = 0; int anoPublicacao = 0; Caixa caixa = caixa; //placeholder

        Revista novaRevista = new Revista(titulo, numeroEdicao, anoPublicacao, caixa);
        return novaRevista;
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