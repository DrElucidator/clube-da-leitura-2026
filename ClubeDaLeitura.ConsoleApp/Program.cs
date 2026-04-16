using ClubeDaLeitura.ConsoleApp.Apresentacao;
using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

RepositorioCaixa repositorioCaixa = new RepositorioCaixa();
TelaCaixa telaCaixa = new TelaCaixa(repositorioCaixa);

RepositorioRevista repositorioRevista = new RepositorioRevista();
TelaRevista telaRevista = new TelaRevista(repositorioRevista);

Caixa caixa = new Caixa("Lançamentos", "Vermelho", 3);
repositorioCaixa.Cadastrar(caixa);

Revista revista = new Revista("Action Comics", 155, 1990, caixa);
repositorioRevista.Cadastrar(revista);

while (true)
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

    if (opcaoMenuPrincipal == "S")
    {
        Console.Clear();
        break;
    }

    while (true)
    {
        string? opcaoMenuInterno = string.Empty;

        if (opcaoMenuPrincipal == "1")
        {
            opcaoMenuInterno = telaCaixa.ObterOpcaoMenu();

            if (opcaoMenuInterno == "S")
            {
                Console.Clear();
                break;
            }
            if (opcaoMenuInterno == "1")
                telaCaixa.Cadastrar();
            else if (opcaoMenuInterno == "2")
                telaCaixa.Editar();
            else if (opcaoMenuInterno == "3")
                telaCaixa.Excluir();
            else if (opcaoMenuInterno == "4")
                telaCaixa.VisualizarTodas(deveExibirCabecalho: true);
            else
                continue;
        }

        else if (opcaoMenuPrincipal == "2")
        {
            opcaoMenuInterno = telaRevista.ObterOpcaoMenu();

            if (opcaoMenuInterno == "S")
            {
                Console.Clear();
                break;
            }
            if (opcaoMenuInterno == "1")
                telaRevista.Cadastrar();
            else if (opcaoMenuInterno == "2")
                telaRevista.Editar();
            else if (opcaoMenuInterno == "3")
                telaRevista.Excluir();
            else if (opcaoMenuInterno == "4")
                telaRevista.VisualizarTodas(deveExibirCabecalho: true);
            else
                continue;
        }

        else if (opcaoMenuPrincipal == "3")
        {

        }

        else if (opcaoMenuPrincipal == "4")
        {

        }
    }
}