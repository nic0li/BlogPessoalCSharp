using BlogPessoal.Model;

namespace BlogPessoal.Repository.Interfaces;

public interface IPublicacaoRepository : IRepository<Publicacao>
{
    Task<IEnumerable<Publicacao>> GetByTitulo(string titulo);
}
