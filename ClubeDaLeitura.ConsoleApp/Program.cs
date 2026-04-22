using ClubeDaLeitura.ConsoleApp.Apresentacao;
using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

RepositorioCaixa repositorioCaixa = new RepositorioCaixa();
RepositorioRevista repositorioRevista = new RepositorioRevista(repositorioCaixa);
RepositorioAmigo repositorioAmigo = new RepositorioAmigo(repositorioRevista);
RepositorioEmprestimo repositorioEmprestimo = new RepositorioEmprestimo();

TelaCaixa telaCaixa = new TelaCaixa(repositorioCaixa);
TelaRevista telaRevista = new TelaRevista(repositorioRevista, repositorioCaixa);
TelaAmigo telaAmigo = new TelaAmigo(repositorioAmigo);
TelaEmprestimo telaEmprestimo = new TelaEmprestimo(repositorioEmprestimo, repositorioRevista, repositorioAmigo, repositorioCaixa);

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

    if (opcaoMenuPrincipal == "S") break;

    while (true)
    {
        string? opcaoMenuInterno = string.Empty;

        if (opcaoMenuPrincipal == "1")
        {
            opcaoMenuInterno = telaCaixa.ObterOpcaoMenu();
            if (opcaoMenuInterno == "S") break;
            if (opcaoMenuInterno == "1") telaCaixa.Cadastrar();
            else if (opcaoMenuInterno == "2") telaCaixa.Editar();
            else if (opcaoMenuInterno == "3") telaCaixa.Excluir();
            else if (opcaoMenuInterno == "4") telaCaixa.VisualizarTodas(true);
        }
        else if (opcaoMenuPrincipal == "2")
        {
            opcaoMenuInterno = telaRevista.ObterOpcaoMenu();
            if (opcaoMenuInterno == "S") break;
            if (opcaoMenuInterno == "1") telaRevista.Cadastrar();
            else if (opcaoMenuInterno == "2") telaRevista.Editar();
            else if (opcaoMenuInterno == "3") telaRevista.Excluir();
            else if (opcaoMenuInterno == "4") telaRevista.VisualizarTodas(true);
        }
        else if (opcaoMenuPrincipal == "3")
        {
            opcaoMenuInterno = telaAmigo.ObterOpcaoMenu();
            if (opcaoMenuInterno == "S") break;
            if (opcaoMenuInterno == "1") telaAmigo.Cadastrar();
            else if (opcaoMenuInterno == "2") telaAmigo.Editar();
            else if (opcaoMenuInterno == "3") telaAmigo.Excluir();
            else if (opcaoMenuInterno == "4") telaAmigo.VisualizarTodas(true);
        }
        else if (opcaoMenuPrincipal == "4")
        {
            opcaoMenuInterno = telaEmprestimo.ObterOpcaoMenuEmprestimos();
            if (opcaoMenuInterno == "S") break;
            if (opcaoMenuInterno == "1") telaEmprestimo.Cadastrar();
            else if (opcaoMenuInterno == "2") telaEmprestimo.Editar();
            else if (opcaoMenuInterno == "3") telaEmprestimo.Excluir();
            else if (opcaoMenuInterno == "4") telaEmprestimo.VisualizarTodas(true);
        }
    }
}