using BlogPessoal.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogPessoal.Service.Interfaces;

namespace BlogPessoal.Security.Implements;

public class AuthService : IAuthService
{
    private readonly IUsuarioService _usuarioService;

    public AuthService(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<UsuarioLogin?> Autenticar(UsuarioLogin usuarioLogin)
    {
        string FotoDefault = "https://i.imgur.com/I8MfmC8.png";

        if (usuarioLogin == null || string.IsNullOrEmpty(usuarioLogin.NomeDeUsuario) || string.IsNullOrEmpty(usuarioLogin.Senha))
            return null;

        var BuscaUsuario = await _usuarioService.GetByUsuario(usuarioLogin.NomeDeUsuario);

        if (BuscaUsuario is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(usuarioLogin.Senha, BuscaUsuario.Senha))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuarioLogin.NomeDeUsuario)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        usuarioLogin.Id = BuscaUsuario.Id;
        usuarioLogin.Nome = BuscaUsuario.Nome;
        usuarioLogin.Foto = BuscaUsuario.Foto is null ? FotoDefault : BuscaUsuario.Foto;
        usuarioLogin.Token = "Bearer " + tokenHandler.WriteToken(token).ToString();
        usuarioLogin.Senha = string.Empty;

        return usuarioLogin;

    }
}

