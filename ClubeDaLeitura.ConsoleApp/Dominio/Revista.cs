using System.Security.Cryptography;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Revista
{
    public string Id { get; set; }
    public string Titulo { get; set; }
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public string IdCaixa { get; set; }
    public Revista(string titulo, int numeroEdicao, int anoPublicacao, string idCaixa)
    {
        Id = Convert
            .ToHexString(RandomNumberGenerator.GetBytes(20))
            .ToUpper()
            .Substring(0, 7);

        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        IdCaixa = idCaixa; 
    }
     public Revista() { }
}