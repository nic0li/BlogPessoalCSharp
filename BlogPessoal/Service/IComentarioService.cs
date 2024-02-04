using BlogPessoal.Model;

namespace BlogPessoal.Service;

public interface IComentarioService
{
    Task<IEnumerable<Comentario>> GetAll();

    Task<Comentario?> GetById(long id);

    Task<IEnumerable<Comentario>> GetByTexto(string texto);

    Task<Comentario?> Create(Comentario comentario);

    Task<Comentario?> Update(Comentario comentario);

    Task Delete(Comentario comentario);
}
