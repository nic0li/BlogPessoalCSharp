using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface IPublicacaoService : IService<Publicacao>
{
    Task<IEnumerable<Publicacao?>> GetByTitulo(string titulo);
}
