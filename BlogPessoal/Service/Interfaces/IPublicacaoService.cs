using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface IPublicacaoService
{
    Task<IEnumerable<Publicacao>> GetAll();

    Task<Publicacao?> GetById(long id);

    Task<Publicacao?> Create(Publicacao entity);

    Task<Publicacao?> Update(Publicacao entity);

    Task Delete(Publicacao entity);

    Task<IEnumerable<Publicacao?>> GetByTitulo(string titulo);
}
