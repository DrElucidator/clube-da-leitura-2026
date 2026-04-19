using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

    public class TelaCaixa : TelaBase<Caixa>
    {
        public override string NomeGestao => "Caixas";
        private readonly RepositorioCaixa repositorioCaixa;

        public TelaCaixa(RepositorioCaixa rC)
        {
            repositorioCaixa = rC;
        }

        public override void Cadastrar()
        {
            ExibirCabecalho("Cadastro de Caixa", NomeGestao);
            Caixa novaCaixa = ObterDadosCadastrais();
            if (novaCaixa == null)
                ExibirMensagem("Cadastro cancelado.");
            else
            {
                repositorioCaixa.Cadastrar(novaCaixa);
                ExibirMensagem($"O registro \"{novaCaixa.Etiqueta}\" foi cadastrado.");
            }
        }

        public override void Editar()
        {
            ExibirCabecalho("Edição de Caixa", NomeGestao);
            VisualizarTodas(false);

            Console.WriteLine("\nDigite o id da caixa para edição: ");
            string? id = Console.ReadLine()?.ToUpper();
            Caixa? caixaEditada = repositorioCaixa.BuscarPorId(id);

            if (caixaEditada == null)
                ExibirMensagem("Caixa não encontrada.");
            else
            {
                Console.Write("Nova etiqueta: ");
                string? novaEtiqueta = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novaEtiqueta))
                    caixaEditada.Etiqueta = novaEtiqueta;

                Console.Write("Novo tempo de empréstimo: ");
                if (int.TryParse(Console.ReadLine(), out int dias))
                    caixaEditada.DiasDeEmprestimo = dias;

                ExibirMensagem($"Caixa \"{caixaEditada.Id}\" editada com sucesso.");
            }
        }

        public override void Excluir()
        {
            ExibirCabecalho("Exclusão de Caixa", NomeGestao);
            VisualizarTodas(false);

            Console.WriteLine("\nDigite o id da caixa para exclusão: ");
            string? id = Console.ReadLine()?.ToUpper();
            Caixa? caixaDeletada = repositorioCaixa.BuscarPorId(id);

            if (caixaDeletada == null)
                ExibirMensagem("Caixa não encontrada.");
            else if (repositorioCaixa.Excluir(id))
                ExibirMensagem($"O registro \"{caixaDeletada.Etiqueta}\" foi excluído.");
            else
                ExibirMensagem("Falha ao excluir: Caixa contém revistas.");
        }

        public override void VisualizarTodas(bool deveExibirCabecalho)
        {
            if (deveExibirCabecalho)
                ExibirCabecalho("Visualização de Caixas", NomeGestao);

            Console.WriteLine("{0, -7} | {1, -20} | {2, -10} | {3, -20}", "Id", "Etiqueta", "Cor", "Dias");
            var caixas = repositorioCaixa.SelecionarTodas();

            if (caixas.Count == 0)
                Console.WriteLine("Nenhuma caixa cadastrada.");
            else
                foreach (var cx in caixas)
                {
                    if (cx.Cor == "Vermelho") Console.ForegroundColor = ConsoleColor.Red;
                    else if (cx.Cor == "Verde") Console.ForegroundColor = ConsoleColor.Green;
                    else if (cx.Cor == "Azul") Console.ForegroundColor = ConsoleColor.Blue;
                    else Console.ResetColor();

                    Console.WriteLine("{0, -7} | {1, -20} | {2, -10} | {3, -20}",
                        cx.Id, cx.Etiqueta, cx.Cor, cx.DiasDeEmprestimo);

                    Console.ResetColor();
                }

            if (deveExibirCabecalho) ExibirMensagem("");
        }

        private Caixa ObterDadosCadastrais()
        {
            Console.Write("Etiqueta: ");
            string etiqueta = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(etiqueta))
            {
                Console.WriteLine("Etiqueta inválida.");
                return null;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("1 = Vermelho");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("2 = Verde");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("3 = Azul");
            Console.ResetColor();
            Console.WriteLine("4 = Branco");

            string? codigoCor = Console.ReadLine();
            string cor = codigoCor switch
            {
                "1" => "Vermelho",
                "2" => "Verde",
                "3" => "Azul",
                _   => "Branco"
            };

            Console.Write("Dias de empréstimo: ");
            int.TryParse(Console.ReadLine(), out int dias);
            if (dias <= 0) dias = 7;

            return new Caixa(etiqueta, cor, dias);
        }
    }