using System.Text.Json;
using ClubeDaLeitura.ConsoleApp.Dominio;
namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

    public abstract class RepositorioBase<T> where T : EntidadeBase
    {
        protected List<T> entidades = new List<T>();
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
            T entidade = entidades.Find(e => e.Id == id);
            if (entidade != null)
            {
                entidades.Remove(entidade);
                SalvarNoArquivo();
                return true;
            }
            return false;
        }

        public List<T> SelecionarTodas()
        {
            return entidades;
        }

        public T? BuscarPorId(string id)
        {
            return entidades.Find(e => e.Id == id);
        }
    }