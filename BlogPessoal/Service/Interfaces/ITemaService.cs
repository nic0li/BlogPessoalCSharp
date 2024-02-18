using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface ITemaService
{
    Task<IEnumerable<Tema>> GetAll();

    Task<Tema?> GetById(long id);

    Task<Tema> Create(Tema entity);

    Task<Tema?> Update(Tema entity);

    Task Delete(Tema entity);

    Task<IEnumerable<Tema>> GetByDescricao(string descricao);
}
