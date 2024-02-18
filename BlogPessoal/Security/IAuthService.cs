using BlogPessoal.Model;

namespace BlogPessoal.Security;

public interface IAuthService
{
    Task<UsuarioLogin?> Autenticar(UsuarioLogin usuarioLogin);
}
