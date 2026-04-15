using ClubeDaLeitura.ConsoleApp.Apresentacao;
using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

RepositorioCaixa repositorioCaixa = new RepositorioCaixa();
TelaCaixa telaCaixa = new TelaCaixa(repositorioCaixa);

Caixa caixa = new Caixa("Lançamentos", "Vermelhor", 3);
repositorioCaixa.Cadastrar(caixa);

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
                telaCaixa.VisualizarTodas();
            else
                continue;
        }

        else if (opcaoMenuPrincipal == "2")
        {

        }

        else if (opcaoMenuPrincipal == "3")
        {

        }

        else if (opcaoMenuPrincipal == "4")
        {

        }
    }
}