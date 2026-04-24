using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaPrincipal
{
    private readonly RepositorioCaixa repositorioCaixa;
    private readonly RepositorioRevista repositorioRevista;
    private readonly RepositorioAmigo repositorioAmigo;
    private readonly RepositorioEmprestimo repositorioEmprestimo;

    public TelaPrincipal(
        RepositorioCaixa repositorioCaixa,
        RepositorioRevista repositorioRevista,
        RepositorioAmigo repositorioAmigo,
        RepositorioEmprestimo repositorioEmprestimo
    )
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public ITela? ApresentarMenuOpcoesPrincipal()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Clube da Leitura");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Gerenciar caixas de revistas");
        Console.WriteLine("2 - Gerenciar revistas");
        Console.WriteLine("3 - Gerenciar amigos");
        Console.WriteLine("4 - Gerenciar empréstimos");
        Console.WriteLine("S - Sair");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");
        string? opcaoMenuPrincipal = Console.ReadLine()?.ToUpper();

        if (opcaoMenuPrincipal == "1")
            return new TelaCaixa(repositorioCaixa);

        if (opcaoMenuPrincipal == "2")
            return new TelaRevista(repositorioRevista, repositorioCaixa);

        if (opcaoMenuPrincipal == "3")
            return new TelaAmigo(repositorioAmigo);

        if (opcaoMenuPrincipal == "4")
            return new TelaEmprestimo(repositorioEmprestimo, repositorioRevista, repositorioAmigo, repositorioCaixa);

        return null;
    }
}