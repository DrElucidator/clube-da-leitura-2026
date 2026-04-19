namespace ClubeDaLeitura.ConsoleApp.Dominio;

    public class Revista : EntidadeBase
    {
        public string Titulo { get; set; }
        public int NumeroEdicao { get; set; }
        public int AnoPublicacao { get; set; }
        public string IdCaixa { get; set; }

        public Revista(string titulo, int numeroEdicao, int anoPublicacao, string idCaixa) : base()
        {
            Titulo = titulo;
            NumeroEdicao = numeroEdicao;
            AnoPublicacao = anoPublicacao;
            IdCaixa = idCaixa;
        }

        public Revista() { }
    }
