using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaAmigo : TelaBase
{
    public override string NomeGestao => "Amigos";
    private readonly RepositorioAmigo repositorioAmigo;
    private readonly RepositorioRevista repositorioRevista;

    public TelaAmigo(RepositorioAmigo repositorioAmigo, RepositorioRevista repositorioRevista)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Amigo", NomeGestao);
        Amigo novoAmigo = ObterDadosCadastrais();
        var erros = novoAmigo.Validar();

        if (erros.Length > 0)
        {
            ExibirErros(erros);
            return;
        }

        bool duplicado = repositorioAmigo.SelecionarTodas()
            .Any(amg => amg.Nome.Equals(novoAmigo.Nome, StringComparison.OrdinalIgnoreCase)
                     || amg.Telefone.Equals(novoAmigo.Telefone));

        if (duplicado)
        {
            ExibirMensagem("Já existe um amigo com o mesmo nome ou telefone.");
            return;
        }

        repositorioAmigo.Cadastrar(novoAmigo);
        ExibirMensagem($"O registro \"{novoAmigo.Nome}\" foi cadastrado.");
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Cadastro de Amigo", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id do amigo para edição: ");
        string? id = Console.ReadLine()?.ToUpper();
        var amigoExistente = repositorioAmigo.BuscarPorId(id);

        if (amigoExistente == null)
        {
            ExibirMensagem("Amigo não encontrado.");
            return;
        }

        Console.Write("Novo nome: ");
        string? novoNome = Console.ReadLine();
        string nomeFinal = string.IsNullOrWhiteSpace(novoNome) ? amigoExistente.Nome : novoNome;

        Console.Write("Novo nome de responsável: ");
        string? novoResponsavel = Console.ReadLine();
        string responsavelFinal = string.IsNullOrWhiteSpace(novoResponsavel) ? amigoExistente.NomeResponsavel : novoResponsavel;

        Console.Write("Novo telefone: ");
        string? novoTelefone = Console.ReadLine();
        string telefoneFinal = string.IsNullOrWhiteSpace(novoTelefone) ? amigoExistente.Telefone : novoTelefone;

        Amigo amigoAtualizado = new Amigo(nomeFinal, responsavelFinal, telefoneFinal);

        var erros = amigoAtualizado.Validar();
        if (erros.Length > 0)
            ExibirErros(erros);
        else
        {
            repositorioAmigo.Editar(id, amigoAtualizado);
            ExibirMensagem($"Amigo \"{id}\" editado com sucesso.");
        }
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Amigo", NomeGestao);
        VisualizarTodas(false);

        Console.WriteLine("\nDigite o id do amigo para exclusão: ");
        string? id = Console.ReadLine()?.ToUpper();
        var amigoDeletado = repositorioAmigo.BuscarPorId(id);

        if (amigoDeletado == null)
            ExibirMensagem("Amigo não encontrado.");
        else if (repositorioAmigo.Excluir(id))
            ExibirMensagem($"O registro de \"{amigoDeletado.Nome}\" foi excluído.");
        else
            ExibirMensagem("Falha na exclusão.");
    }

    public override void VisualizarTodas(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Amigos", NomeGestao);

        Console.WriteLine("{0,-7} | {1,-15} | {2,-15} | {3,-15} | {4,-20}",
            "Id", "Nome", "Responsável", "Telefone", "Qtd Revistas Emprestadas");

        var amigos = repositorioAmigo.SelecionarTodas();
        if (amigos.Count == 0)
            Console.WriteLine("Nenhum amigo cadastrado.");
        else
            foreach (var amg in amigos)
            {
                int qtdRevistasEmprestadas = repositorioRevista.SelecionarTodas()
                    .Count(r => r.IdAmigoEmprestado == amg.Id);

                Console.WriteLine("{0,-7} | {1,-15} | {2,-15} | {3,-15} | {4,-20}",
                    amg.Id, amg.Nome, amg.NomeResponsavel, amg.TelefoneFormatado, qtdRevistasEmprestadas);
            }

        if (deveExibirCabecalho) ExibirMensagem("");
    }

    private Amigo ObterDadosCadastrais()
    {
        Console.Write("Digite o nome do amigo: ");
        string nome = Console.ReadLine() ?? "";

        Console.Write("Digite o nome do responsável: ");
        string nomeResponsavel = Console.ReadLine() ?? "";

        Console.Write("Digite o telefone: ");
        string telefone = Console.ReadLine() ?? "";

        return new Amigo(nome, nomeResponsavel, telefone);
    }
}