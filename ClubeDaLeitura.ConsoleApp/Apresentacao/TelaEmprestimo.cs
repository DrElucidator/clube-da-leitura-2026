using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao
{
    public class TelaEmprestimo : TelaBase<Emprestimo>
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

            var erros = emprestimo.Validar();
            if (erros.Length > 0)
            {
                ExibirErros(erros);
                return;
            }

            repositorioEmprestimo.Cadastrar(emprestimo);
            ExibirMensagem($"Empréstimo da revista \"{emprestimo.Revista.Titulo}\" para \"{emprestimo.Amigo.Nome}\" cadastrado.");
        }

        public override void Editar()
        {
            ExibirCabecalho("Atualização de Empréstimo", NomeGestao);
            VisualizarTodas(false);

            Console.Write("Digite o ID do empréstimo: ");
            string id = Console.ReadLine()?.ToUpper();
            Emprestimo emprestimoExistente = repositorioEmprestimo.BuscarPorId(id);

            if (emprestimoExistente == null)
            {
                ExibirMensagem("Empréstimo não encontrado.");
                return;
            }

            Emprestimo emprestimoAtualizado = ObterDadosCadastrais();
            if (emprestimoAtualizado == null)
            {
                ExibirMensagem("Atualização cancelada.");
                return;
            }

            emprestimoExistente.AtualizarRegistro(emprestimoAtualizado);

            repositorioEmprestimo.Atualizar(emprestimoExistente);

            ExibirMensagem("Empréstimo atualizado com sucesso.");
        }

        public override void Excluir()
        {
            ExibirCabecalho("Exclusão de Empréstimo", NomeGestao);
            VisualizarTodas(false);

            Console.Write("Digite o ID do empréstimo: ");
            string id = Console.ReadLine()?.ToUpper();

            if (repositorioEmprestimo.Excluir(id))
                ExibirMensagem("Empréstimo excluído.");
            else
                ExibirMensagem("Empréstimo não encontrado.");
        }

        public override void VisualizarTodas(bool deveExibirCabecalho)
        {
            if (deveExibirCabecalho)
                ExibirCabecalho("Visualização de Empréstimos", NomeGestao);

            Console.WriteLine("{0,-7} | {1,-25} | {2,-20} | {3,-12} | {4,-12} | {5,-10} | {6,-10}",
                "Id", "Revista", "Amigo", "Abertura", "Prazo", "Dias", "Status");

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

                DateTime prazo = e.CalcularPrazoRetorno(caixa);
                int dias = e.DiasDecorridos();

                if (e.Status == StatusEmprestimo.Retornado)
                    Console.ForegroundColor = ConsoleColor.Gray;
                else if (e.Status == StatusEmprestimo.Atrasado)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("{0,-7} | {1,-25} | {2,-20} | {3,-12:dd/MM/yyyy} | {4,-12:dd/MM/yyyy} | {5,-10} | {6,-10}",
                    e.Id, e.Revista.Titulo, e.Amigo.Nome, e.Abertura, prazo, dias, e.Status);

                Console.ResetColor();
            }

            if (deveExibirCabecalho) ExibirMensagem("");
        }

        private Emprestimo ObterDadosCadastrais()
        {
            Console.WriteLine("Selecione o amigo:");
            var amigos = repositorioAmigo.SelecionarTodos();
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
    }
}