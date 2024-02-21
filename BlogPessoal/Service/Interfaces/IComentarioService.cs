using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface IComentarioService : IService<Comentario>
{
    Task<IEnumerable<Comentario?>> GetByTexto(string texto);
}
