namespace ClubeDaLeitura.ConsoleApp.Dominio;

public enum StatusEmprestimo
{
    Indefinido,
    Aberto,
    Atrasado,
    Retornado
}

public class Emprestimo : EntidadeBase
{
    public Revista Revista { get; set; }
    public Amigo Amigo { get; set; }
    public DateTime Abertura { get; set; }
    public DateTime? DataDevolucao { get; set; }

    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Indefinido;

    public Emprestimo(Revista revista, Amigo amigo)
    {
        Revista = revista;
        Amigo = amigo;
        Abertura = DateTime.Now;
        Status = StatusEmprestimo.Aberto;
    }

    public DateTime CalcularPrazoRetorno(Caixa caixa)
    {
        if (caixa == null)
            return DateTime.MinValue;

        return Abertura.AddDays(caixa.DiasDeEmprestimo);
    }

    public int DiasDecorridos()
    {
        return (DateTime.Now - Abertura).Days;
    }

    public void AtualizarStatus(Caixa caixa)
    {
        if (Status == StatusEmprestimo.Retornado)
            return;

        DateTime prazo = CalcularPrazoRetorno(caixa);

        if (DateTime.Now > prazo)
            Status = StatusEmprestimo.Atrasado;
        else
            Status = StatusEmprestimo.Aberto;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (Revista == null)
            erros += "O campo \"Revista\" deve ser preenchido;";

        if (Amigo == null)
            erros += "O campo \"Amigo\" deve ser preenchido;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }

    public override void AtualizarRegistro(EntidadeBase novoRegistro)
    {
        Emprestimo atualizado = (Emprestimo)novoRegistro;

        Revista = atualizado.Revista;
        Amigo = atualizado.Amigo;
        Abertura = atualizado.Abertura;
        Status = atualizado.Status;
    }
}