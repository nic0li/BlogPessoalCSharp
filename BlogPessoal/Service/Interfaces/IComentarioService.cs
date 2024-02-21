using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface IComentarioService
{
    Task<IEnumerable<Comentario>> GetAll();

    Task<Comentario?> GetById(long id);

    Task<Comentario?> Create(Comentario entity);

    Task<Comentario?> Update(Comentario entity);

    Task Delete(Comentario entity);

    Task<IEnumerable<Comentario?>> GetByTexto(string texto);
}
