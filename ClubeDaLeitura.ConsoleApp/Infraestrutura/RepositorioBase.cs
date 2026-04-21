using System.Text.Json;
using ClubeDaLeitura.ConsoleApp.Dominio;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public abstract class RepositorioBase<T> where T : EntidadeBase
{
    protected List<T> entidades = new();
    private readonly string caminhoArquivo;

    protected RepositorioBase(string caminhoArquivo)
    {
        this.caminhoArquivo = caminhoArquivo;
        CarregarDoArquivo();
    }

    private void CarregarDoArquivo()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            entidades = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }

    public void SalvarNoArquivo()
    {
        string json = JsonSerializer.Serialize(entidades, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(caminhoArquivo, json);
    }

    public void Cadastrar(T entidade)
    {
        entidades.Add(entidade);
        SalvarNoArquivo();
    }

    public virtual bool Excluir(string id)
    {
        var entidade = BuscarPorId(id);
        if (entidade != null)
        {
            entidades.Remove(entidade);
            SalvarNoArquivo();
            return true;
        }
        return false;
    }

    public List<T> SelecionarTodas() => entidades;

    public T? BuscarPorId(string id) => entidades.Find(e => e.Id == id);

    public bool Editar(string id, T entidadeAtualizada)
    {
        var entidade = BuscarPorId(id);
        if (entidade != null)
        {
            entidade.AtualizarRegistro(entidadeAtualizada);
            SalvarNoArquivo();
            return true;
        }
        return false;
    }

    public bool Existe(string id) => entidades.Any(e => e.Id == id);
}