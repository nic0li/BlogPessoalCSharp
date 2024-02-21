using BlogPessoal.Model;

namespace BlogPessoal.Repository.Interfaces;

public interface ITemaRepository : IRepository<Tema>
{
    Task<IEnumerable<Tema>> GetByDescricao(string descricao);
}
