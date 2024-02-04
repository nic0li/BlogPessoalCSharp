using BlogPessoal.Model;

namespace BlogPessoal.Service;

public interface IPublicacaoService
{
    Task<IEnumerable<Publicacao>> GetAll();

    Task<Publicacao?> GetById(long id);

    Task<IEnumerable<Publicacao>> GetByTitulo(string titulo);

    Task<Publicacao?> Create(Publicacao publicacao);

    Task<Publicacao?> Update(Publicacao publicacao);

    Task Delete(Publicacao publicacao);
}
