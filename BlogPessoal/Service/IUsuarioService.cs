using BlogPessoal.Model;

namespace BlogPessoal.Service;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAll();

    Task<Usuario?> GetById(long id);

    Task<Usuario?> GetByUsuario(string usuario);

    Task<Usuario?> Create(Usuario usuario);

    Task<Usuario?> Update(Usuario usuario);
}
