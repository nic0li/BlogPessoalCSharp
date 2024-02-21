using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface ITemaService : IService<Tema>
{
    Task<IEnumerable<Tema?>> GetByDescricao(string descricao);
}
