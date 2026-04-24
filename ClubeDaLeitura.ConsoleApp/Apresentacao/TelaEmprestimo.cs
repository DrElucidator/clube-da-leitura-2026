using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaEmprestimo : TelaBase
{
    public override string NomeGestao => "Empréstimos";

    private readonly RepositorioEmprestimo repositorioEmprestimo;
    private readonly RepositorioRevista repositorioRevista;
    private readonly RepositorioAmigo repositorioAmigo;
    private readonly RepositorioCaixa repositorioCaixa;

    public TelaEmprestimo(
        RepositorioEmprestimo rE,
        RepositorioRevista rR,
        RepositorioAmigo rA,
        RepositorioCaixa rC)
    {
        repositorioEmprestimo = rE;
        repositorioRevista = rR;
        repositorioAmigo = rA;
        repositorioCaixa = rC;
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Empréstimo", NomeGestao);

        Emprestimo emprestimo = ObterDadosCadastrais();
        if (emprestimo == null)
        {
            ExibirMensagem("Cadastro cancelado.");
            return;
        }

        if (!emprestimo.Revista.Disponivel)
        {
            ExibirMensagem("Essa revista já está emprestada e não pode ser emprestada novamente.");
            return;
        }

        var erros = emprestimo.Validar();
        if (erros.Length > 0)
        {
            ExibirErros(erros);
            return;
        }

        emprestimo.Revista.Disponivel = false;
        emprestimo.Revista.IdAmigoEmprestado = emprestimo.Amigo.Id;
        repositorioRevista.Atualizar(emprestimo.Revista);

        repositorioEmprestimo.Cadastrar(emprestimo);

        ExibirMensagem($"Empréstimo da revista \"{emprestimo.Revista.Titulo}\" para \"{emprestimo.Amigo.Nome}\" cadastrado.");
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Empréstimo", NomeGestao);
        VisualizarTodas(false);

        Console.Write("Digite o ID do empréstimo: ");
        string id = Console.ReadLine()?.ToUpper();
        Emprestimo emprestimoExistente = repositorioEmprestimo.BuscarPorId(id);

        if (emprestimoExistente == null)
        {
            ExibirMensagem("Empréstimo não encontrado.");
            return;
        }

        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("1 - Estender prazo de devolução");
        Console.WriteLine("2 - Transferir posse para outro amigo");
        string opcaoEditar = Console.ReadLine();

        switch (opcaoEditar)
        {
            case "1":
                Console.Write("Quantos dias deseja adicionar ao prazo? ");
                string diasExtras = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(diasExtras) && int.TryParse(diasExtras, out int diasExtra))
                {
                    emprestimoExistente.Abertura = emprestimoExistente.Abertura.AddDays(-diasExtra);
                }
                break;

            case "2":
                Console.Write("Digite o ID do novo amigo em posse da revista: ");
                string idAmigo = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(idAmigo))
                {
                    Amigo novoAmigo = repositorioAmigo.BuscarPorId(idAmigo.ToUpper());
                    if (novoAmigo != null)
                    {
                        emprestimoExistente.Amigo = novoAmigo;
                        emprestimoExistente.Revista.IdAmigoEmprestado = novoAmigo.Id;

                        repositorioRevista.Atualizar(emprestimoExistente.Revista);
                    }
                }
                break;

            default:
                ExibirMensagem("Opção inválida.");
                return;
        }

        repositorioEmprestimo.Atualizar(emprestimoExistente);
        ExibirMensagem("Empréstimo atualizado com sucesso.");
    }

    public override void Excluir()
    {
        ExibirCabecalho("Retorno de Empréstimo", NomeGestao);
        VisualizarTodas(false);

        Console.Write("Digite o ID do empréstimo: ");
        string id = Console.ReadLine()?.ToUpper();
        Emprestimo emprestimoExistente = repositorioEmprestimo.BuscarPorId(id);

        if (emprestimoExistente == null)
        {
            ExibirMensagem("Empréstimo não encontrado.");
            return;
        }

        emprestimoExistente.Status = StatusEmprestimo.Retornado;
        emprestimoExistente.Revista.Disponivel = true;
        emprestimoExistente.Revista.IdAmigoEmprestado = null;

        repositorioRevista.Atualizar(emprestimoExistente.Revista);
        repositorioEmprestimo.Atualizar(emprestimoExistente);

        ExibirMensagem("Revista retornada com sucesso.");
    }

    public override void VisualizarTodas(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Empréstimos", NomeGestao);

        Console.WriteLine("{0,-7} | {1,-25} | {2,-20} | {3,-8} | {4,-10} | {5,-10} | {6,-10} | {7,-12}",
            "Id", "Revista", "Amigo", "Abertura", "Dias", "Status", "Disponível", "AmigoId");

        var emprestimos = repositorioEmprestimo.SelecionarTodos();
        if (emprestimos.Count == 0)
        {
            Console.WriteLine("Nenhum empréstimo cadastrado.");
            return;
        }

        foreach (var e in emprestimos)
        {
            Caixa caixa = repositorioCaixa.BuscarPorId(e.Revista.IdCaixa);
            e.AtualizarStatus(caixa);

            int dias = e.DiasDecorridos();
            string disponivel = e.Revista.Disponivel ? "Sim" : "Não";

            if (e.Status == StatusEmprestimo.Retornado)
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (e.Status == StatusEmprestimo.Atrasado)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("{0,-7} | {1,-25} | {2,-20} | {3,-8:dd/MM} | {4,-10} | {5,-10} | {6,-10} | {7,-12}",
                e.Id, e.Revista.Titulo, e.Amigo.Nome, e.Abertura, dias, e.Status, disponivel, e.Revista.IdAmigoEmprestado ?? "-");

            Console.ResetColor();
        }

        if (deveExibirCabecalho) ExibirMensagem("");
    }

    private Emprestimo ObterDadosCadastrais()
    {
        Console.WriteLine("Selecione o amigo:");
        var amigos = repositorioAmigo.SelecionarTodas();
        foreach (var a in amigos)
            Console.WriteLine($"{a.Id} - {a.Nome}");

        Console.Write("Digite o ID do amigo: ");
        string idAmigo = Console.ReadLine()?.ToUpper();
        Amigo amigo = repositorioAmigo.BuscarPorId(idAmigo);

        Console.WriteLine("Selecione a revista:");
        var revistas = repositorioRevista.SelecionarTodas();
        foreach (var r in revistas)
            Console.WriteLine($"{r.Id} - {r.Titulo}");

        Console.Write("Digite o ID da revista: ");
        string idRevista = Console.ReadLine()?.ToUpper();
        Revista revista = repositorioRevista.BuscarPorId(idRevista);

        if (amigo == null || revista == null)
        {
            Console.WriteLine("Amigo ou revista inválido.");
            return null;
        }

        return new Emprestimo(revista, amigo);
    }

    protected static void ExibirErros(string[] erros)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var erro in erros)
            Console.WriteLine($"Erro: {erro}");
        Console.ResetColor();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadKey();
    }
    public string? ObterOpcaoMenuEmprestimos()
    {
        Console.Clear();
        Console.WriteLine("Gestão de Empréstimos");
        Console.WriteLine("1 - Cadastrar");
        Console.WriteLine("2 - Editar");
        Console.WriteLine("3 - Excluir");
        Console.WriteLine("4 - Visualizar");
        Console.WriteLine("5 - Retornar Empréstimo");
        Console.WriteLine("S - Voltar");
        Console.Write("> ");
        return Console.ReadLine()?.ToUpper();
    }
}