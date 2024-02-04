using BlogPessoal.Model;

namespace BlogPessoal.Security;

public interface IAuthService
{
    Task<Login?> Autenticar(Login usuarioLogin);
}
