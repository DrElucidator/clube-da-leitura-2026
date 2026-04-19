using System.Security.Cryptography;
using System.Text.Json.Serialization;
namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Caixa
{
    public string Id { get; set; } 
    public string Etiqueta { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int DiasDeEmprestimo { get; set; } = 7;

    public List<string> RevistaIds { get; set; } = new List<string>();

    [JsonIgnore]
    
    public List<Revista> Revistas { get; set; } = new List<Revista>();

    public Caixa(string etiqueta, string cor, int diasDeEmprestimo)
    {
        Id = Convert
            .ToHexString(RandomNumberGenerator.GetBytes(20))
            .ToUpper()
            .Substring(0, 5);

        Etiqueta = etiqueta;
        Cor = cor;
        DiasDeEmprestimo = diasDeEmprestimo;
    }

    public Caixa() { }
}
