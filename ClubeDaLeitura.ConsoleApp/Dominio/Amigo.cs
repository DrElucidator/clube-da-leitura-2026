namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Amigo : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = nome;
        NomeResponsavel = nomeResponsavel;
        Telefone = telefone;
    }

    public Amigo() { }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        var amigoAtualizado = (Amigo)entidadeAtualizada;
        Nome = amigoAtualizado.Nome;
        NomeResponsavel = amigoAtualizado.NomeResponsavel;
        Telefone = amigoAtualizado.Telefone;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;
        string apenasDigitos = new string(Telefone.Where(char.IsDigit).ToArray());

        if (apenasDigitos.Length < 10 || apenasDigitos.Length > 11)
            erros += "O campo \"Telefone\" deve conter entre 10 e 11 dígitos;";

        if (!apenasDigitos.All(char.IsDigit))
            erros += "O campo \"Telefone\" deve conter apenas números;";

        if (string.IsNullOrWhiteSpace(Nome))
            erros += "O campo \"Nome\" deve ser preenchido;";

        if (string.IsNullOrWhiteSpace(NomeResponsavel))
            erros += "O campo \"Nome do Responsável\" deve ser preenchido;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }

    public string TelefoneFormatado
    {
        get
        {
            string apenasDigitos = new string(Telefone.Where(char.IsDigit).ToArray());
            if (apenasDigitos.Length == 11)
                return $"({apenasDigitos[..2]}) {apenasDigitos.Substring(2, 5)}-{apenasDigitos.Substring(7)}";
            if (apenasDigitos.Length == 10)
                return $"({apenasDigitos[..2]}) {apenasDigitos.Substring(2, 4)}-{apenasDigitos.Substring(6)}";
            return Telefone;
        }
    }
}