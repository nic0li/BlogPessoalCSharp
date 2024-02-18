using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAll();

    Task<Usuario?> GetById(long id);

    Task<Usuario> Create(Usuario entity);

    Task<Usuario?> Update(Usuario entity);

    Task Delete(Usuario entity);

    Task<Usuario?> GetByUsuario(string usuario);
}
