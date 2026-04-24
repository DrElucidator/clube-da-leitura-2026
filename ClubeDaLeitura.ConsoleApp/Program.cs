using ClubeDaLeitura.ConsoleApp.Apresentacao;
using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

RepositorioCaixa repositorioCaixa = new RepositorioCaixa();
RepositorioRevista repositorioRevista = new RepositorioRevista(repositorioCaixa);
RepositorioAmigo repositorioAmigo = new RepositorioAmigo(repositorioRevista);
RepositorioEmprestimo repositorioEmprestimo = new RepositorioEmprestimo();

TelaPrincipal telaPrincipal = new TelaPrincipal(
    repositorioCaixa,
    repositorioRevista,
    repositorioAmigo,
    repositorioEmprestimo
);

while (true)
{
    ITela? telaSelecionada = telaPrincipal.ApresentarMenuOpcoesPrincipal();

    if (telaSelecionada == null)
    {
        Console.Clear();
        break;
    }

    while (true)
    {
        string? opcaoMenuInterno;

        if (telaSelecionada is TelaEmprestimo telaEmprestimo)
        {
            opcaoMenuInterno = telaEmprestimo.ObterOpcaoMenuEmprestimos();
            if (opcaoMenuInterno == "S") break;
            if (opcaoMenuInterno == "1") telaEmprestimo.Cadastrar();
            else if (opcaoMenuInterno == "2") telaEmprestimo.Editar();
            else if (opcaoMenuInterno == "3") telaEmprestimo.Excluir();
            else if (opcaoMenuInterno == "4") telaEmprestimo.VisualizarTodas(true);
        }
        
        else if (telaSelecionada is TelaBase telaBase)
        {
            opcaoMenuInterno = telaBase.ObterOpcaoMenu();
            if (opcaoMenuInterno == "S") break;

            if (opcaoMenuInterno == "1") telaBase.Cadastrar();
            else if (opcaoMenuInterno == "2") telaBase.Editar();
            else if (opcaoMenuInterno == "3") telaBase.Excluir();
            else if (opcaoMenuInterno == "4") telaBase.VisualizarTodas(true);
        }
    }
}