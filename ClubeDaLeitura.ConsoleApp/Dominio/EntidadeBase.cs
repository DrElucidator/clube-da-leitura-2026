using System.Security.Cryptography;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public abstract class EntidadeBase
{
    public string Id { get; set; }

    protected EntidadeBase()
    {
        if (string.IsNullOrEmpty(Id))
            Id = GerarId();
    }

    private string GerarId()
    {
        return Convert
            .ToHexString(RandomNumberGenerator.GetBytes(20))
            .ToUpper()
            .Substring(0, 5);
    }

    public abstract void AtualizarRegistro(EntidadeBase entidadeAtualizada);
    public abstract string[] Validar();
}