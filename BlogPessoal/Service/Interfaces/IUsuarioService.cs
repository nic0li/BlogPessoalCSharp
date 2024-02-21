using BlogPessoal.Model;

namespace BlogPessoal.Service.Interfaces;

public interface IUsuarioService : IService<Usuario>
{
    Task<Usuario?> GetByUsuario(string usuario);
}
