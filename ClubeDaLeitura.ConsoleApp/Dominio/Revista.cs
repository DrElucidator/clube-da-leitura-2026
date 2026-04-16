using System.Security.Cryptography;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Revista
{
    public string Id { get; private set; }
    public string Titulo { get; set; }
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public Caixa Caixa { get; set; }
    public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa)
    {
        Id = Convert
            .ToHexString(RandomNumberGenerator.GetBytes(20))
            .ToUpper()
            .Substring(0, 7);

        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        Caixa = caixa; 
    }
}