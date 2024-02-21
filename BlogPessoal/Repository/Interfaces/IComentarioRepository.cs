using BlogPessoal.Model;

namespace BlogPessoal.Repository.Interfaces;

public interface IComentarioRepository : IRepository<Comentario>
{
    Task<IEnumerable<Comentario>> GetByTexto(string texto);
}
