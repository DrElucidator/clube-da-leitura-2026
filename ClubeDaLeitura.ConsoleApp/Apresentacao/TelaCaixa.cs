using System.Security.Cryptography.X509Certificates;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
using ClubeDaLeitura.ConsoleApp.Dominio;
using System.Net;
using System.Data.Common;
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
        string? opcaoMenu = Console.ReadLine()?.ToUpper();

        return opcaoMenu;
    }

    public void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Caixa");

        Caixa novaCaixa = ObterDadosCadastrais();

        if (novaCaixa == null)
        {
            ExibirMensagem("Cadastro cancelado.");
        }
        else
        {
            repositorioCaixa.Cadastrar(novaCaixa);
            ExibirMensagem($"O registro \"{novaCaixa.Etiqueta}\" foi cadastrado.");
        }
    }
    public void Editar()
    {
        ExibirCabecalho("Edição de Caixa");

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
        {
            foreach (Caixa c in caixas)
            {
                Console.WriteLine(
            "{0, 7} | {1, -20} | {2, -10} | {3, -20}",
            c.Id, c.Etiqueta, c.Cor, c.DiasDeEmprestimo
                );
            }
        }

        Console.WriteLine("\nDigite o id da caixa para edição: ");
        string? id = Console.ReadLine().ToUpper();

        Caixa? caixaEditada = repositorioCaixa.BuscarPorId(id);

        if (caixaEditada == null)
            Console.WriteLine("Caixa não encontrada.");
        else
        {
            Console.WriteLine($"Caixa \"{caixaEditada.Etiqueta}\" foi selecionada para edição");

            Console.Write("Digite a nova etiqueta ou deixe em branco para manter a mesma: ");
            string? novaEtiqueta = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novaEtiqueta))
            {
                caixaEditada.Etiqueta = novaEtiqueta;
            }

            Console.Write("Deseja modificar a cor da caixa? [S/N]");
            string? novaCor = Console.ReadLine()?.ToUpper();

            if (!string.IsNullOrWhiteSpace(novaCor) && novaCor == "S")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1 = Vermelho");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2 = Verde");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("3 = Azul");
                Console.ResetColor();
                Console.WriteLine("4 = Branco\n");

                string? codigoCor = Console.ReadLine();
                string cor;

                if (codigoCor == "1")
                    cor = "Vermelho";
                else if (codigoCor == "2")
                    cor = "Verde";
                else if (codigoCor == "3")
                    cor = "Azul";
                else
                    cor = "Branco";
            }

            Console.Write("Digite o novo tempo de empréstimo: ");
            string valorDiasDeEmprestimo = Console.ReadLine();

            int novaDiasDeEmprestimo;

            if (string.IsNullOrWhiteSpace(valorDiasDeEmprestimo))
            {
                novaDiasDeEmprestimo = 7;
            }
            else if (!int.TryParse(valorDiasDeEmprestimo, out novaDiasDeEmprestimo))
            {
                Console.WriteLine("\nValor inválido, portanto valor padrão de 7 dias atribuído.\n");
                novaDiasDeEmprestimo = 7;
            }
            caixaEditada.DiasDeEmprestimo = novaDiasDeEmprestimo;

            ExibirMensagem($"Caixa \"{caixaEditada.Id}\" foi editada com sucesso.");
        }
    }

    public void Excluir()
    {
        ExibirCabecalho("Exclusão de Caixa");

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
        {
            foreach (Caixa cx in caixas)
            {
                Console.WriteLine(
            "{0, 7} | {1, -20} | {2, -10} | {3, -20}",
            cx.Id, cx.Etiqueta, cx.Cor, cx.DiasDeEmprestimo
                );
            }
        }
        Console.WriteLine("\nDigite o id da caixa para exclusão: ");
        string? id = Console.ReadLine().ToUpper();

        Caixa? caixaDeletada = repositorioCaixa.BuscarPorId(id);

        if (caixaDeletada == null)
            Console.WriteLine("Caixa não encontrada.");
        else
            if (caixaDeletada.Revistas.Count == 0)
            {
                repositorioCaixa.Excluir(id);
                ExibirMensagem($"O registro \"{caixaDeletada.Etiqueta}\" foi excluido.");
            }
            else
                ExibirMensagem("Não é possível excluir uma caixa que contenha revistas.");
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
            Console.WriteLine("\nNenhuma caixa cadastrada.");
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
            ExibirMensagem("");
        }
    }
    private Caixa ObterDadosCadastrais()
    {
        ExibirCabecalho("Digite a etiqueta da caixa: ");
        string etiqueta = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(etiqueta) || etiqueta.Length > 50)
        {
            Console.WriteLine("Nome da caixa inválido, digite um nome válido de até 50 caracteres.");
            return null;
        }

        Console.WriteLine("\nSelecione uma das cores");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1 = Vermelho");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("2 = Verde");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("3 = Azul");
        Console.ResetColor();
        Console.WriteLine("4 = Branco\n");

        string? codigoCor = Console.ReadLine();
        string cor;

        if (codigoCor == "1")
            cor = "Vermelho";
        else if (codigoCor == "2")
            cor = "Verde";
        else if (codigoCor == "3")
            cor = "Azul";
        else
            cor = "Branco";

        Console.WriteLine("informe o tempo de empréstimo das revistas desta caixa: ");
        string quantidadeDiasDeEmprestimo = Console.ReadLine();

        int diasDeEmprestimo;

        if (string.IsNullOrWhiteSpace(quantidadeDiasDeEmprestimo))
        {
            diasDeEmprestimo = 7;
        }
        else if (!int.TryParse(quantidadeDiasDeEmprestimo, out diasDeEmprestimo))
        {
            Console.WriteLine("\nValor inválido, portanto valor padrão de 7 dias atribuído.\n");
            diasDeEmprestimo = 7;
        }

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