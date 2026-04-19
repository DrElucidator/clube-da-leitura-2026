namespace ClubeDaLeitura.ConsoleApp.Apresentacao;
    public abstract class TelaBase<T>
    {
        public abstract string NomeGestao { get; }

        public virtual string? ObterOpcaoMenu()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"Gestão de {NomeGestao}");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Editar");
            Console.WriteLine("3 - Excluir");
            Console.WriteLine("4 - Visualizar");
            Console.WriteLine("S - Voltar");
            Console.WriteLine("---------------------------------");
            Console.Write("> ");
            return Console.ReadLine()?.ToUpper();
        }

        protected static void ExibirCabecalho(string titulo, string nomeGestao)
        {
            Console.Clear();
            Console.WriteLine($"\nGestão de {nomeGestao}\n");
            Console.WriteLine(titulo, "\n");
        }

        protected static void ExibirMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadKey();
        }

        public abstract void Cadastrar();
        public abstract void Editar();
        public abstract void Excluir();
        public abstract void VisualizarTodas(bool deveExibirCabecalho);
    }