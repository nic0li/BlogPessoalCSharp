using BlogPessoal.Model;

namespace BlogPessoal.Repository.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByUsuario(string usuario);
}
