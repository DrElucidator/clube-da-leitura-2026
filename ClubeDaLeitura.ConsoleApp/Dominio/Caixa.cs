namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Caixa : EntidadeBase
{
    public string Etiqueta { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int DiasDeEmprestimo { get; set; } = 7;

    public List<string> RevistaIds { get; set; } = new();
    public List<Revista> Revistas { get; set; } = new();

    public Caixa(string etiqueta, string cor, int diasDeEmprestimo)
    {
        Etiqueta = etiqueta;
        Cor = cor;
        DiasDeEmprestimo = diasDeEmprestimo;
    }

    public Caixa() { }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        var caixaAtualizada = (Caixa)entidadeAtualizada;
        Etiqueta = caixaAtualizada.Etiqueta;
        Cor = caixaAtualizada.Cor;
        DiasDeEmprestimo = caixaAtualizada.DiasDeEmprestimo;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (string.IsNullOrWhiteSpace(Etiqueta))
            erros += "O campo \"Etiqueta\" deve ser preenchido;";

        if (string.IsNullOrWhiteSpace(Cor))
            erros += "O campo \"Cor\" deve ser preenchido;";

        if (DiasDeEmprestimo <= 0)
            erros += "O campo \"Dias de Empréstimo\" deve ser maior que zero;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }
}