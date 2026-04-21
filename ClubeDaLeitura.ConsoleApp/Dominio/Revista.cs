namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Revista : EntidadeBase
{
    public string Titulo { get; set; } = string.Empty;
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public string IdCaixa { get; set; } = string.Empty;
    public string? IdAmigoEmprestado { get; set; }

    public Revista(string titulo, int numeroEdicao, int anoPublicacao, string idCaixa)
    {
        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        IdCaixa = idCaixa;
    }

    public Revista() { }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        var revistaAtualizada = (Revista)entidadeAtualizada;
        Titulo = revistaAtualizada.Titulo;
        NumeroEdicao = revistaAtualizada.NumeroEdicao;
        AnoPublicacao = revistaAtualizada.AnoPublicacao;
        IdCaixa = revistaAtualizada.IdCaixa;
        IdAmigoEmprestado = revistaAtualizada.IdAmigoEmprestado;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (string.IsNullOrWhiteSpace(Titulo))
            erros += "O campo \"Título\" deve ser preenchido;";

        if (NumeroEdicao <= 0)
            erros += "O campo \"Número da Edição\" deve ser maior que zero;";

        if (AnoPublicacao < 1900 || AnoPublicacao > DateTime.Now.Year)
            erros += "O campo \"Ano de Publicação\" deve ser válido;";

        if (string.IsNullOrWhiteSpace(IdCaixa))
            erros += "A revista deve estar vinculada a uma caixa;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }
}