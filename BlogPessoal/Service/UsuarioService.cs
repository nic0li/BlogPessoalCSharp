using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using BlogPessoal.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Service;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly IUsuarioRepository _repository;

    public UsuarioService(AppDbContext context, IUsuarioRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<IEnumerable<Usuario>> GetAll()
    {
        return await _context.Usuarios
            .Include(u => u.Publicacao)
            .ToListAsync();
    }

    public async Task<Usuario?> GetById(long id)
    {
        try
        {
            var Usuario = await _context.Usuarios
                .Include(u => u.Publicacao)
                .FirstAsync(u => u.Id == id);

            Usuario.Senha = "";

            return Usuario;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Usuario?> GetByUsuario(string usuario)
    {
        try
        {
            var BuscaUsuario = await _context.Usuarios
                .Include(u => u.Publicacao)
                .Where(u => u.NomeDeUsuario == usuario)
                .FirstOrDefaultAsync();

            return BuscaUsuario;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Usuario?> Create(Usuario usuario)
    {
        var BuscaUsuario = await GetByUsuario(usuario.NomeDeUsuario);

        if (BuscaUsuario is not null)
            return null;

        if (usuario.Foto is null || usuario.Foto == "")
            usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<Usuario?> Update(Usuario usuario)
    {
        var UsuarioUpdate = await _context.Usuarios.FindAsync(usuario.Id);

        if (UsuarioUpdate is null)
            return null;

        if (usuario.Foto is null || usuario.Foto == "")
            usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

        _context.Entry(UsuarioUpdate).State = EntityState.Detached;
        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task Delete(Usuario entity)
    {
        _context.Usuarios.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
