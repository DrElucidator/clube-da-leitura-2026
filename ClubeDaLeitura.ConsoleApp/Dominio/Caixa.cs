namespace ClubeDaLeitura.ConsoleApp.Dominio;

    public class Caixa : EntidadeBase
    {
        public string Etiqueta { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public int DiasDeEmprestimo { get; set; } = 7;

        public List<string> RevistaIds { get; set; } = new List<string>();
        public List<Revista> Revistas { get; set; } = new List<Revista>();

        public Caixa(string etiqueta, string cor, int diasDeEmprestimo) : base()
        {
            Etiqueta = etiqueta;
            Cor = cor;
            DiasDeEmprestimo = diasDeEmprestimo;
        }

        public Caixa() { }
    }