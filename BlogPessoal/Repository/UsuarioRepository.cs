using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private AppDbContext _dbContext;

    public UsuarioRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Usuario>> GetAll()
    {
        return await _dbContext.Usuarios
            .Include(u => u.Publicacao)
            .ToListAsync();
    }

    public async Task<Usuario?> GetById(long id)
    {
        try
        {
            var Usuario = await _dbContext.Usuarios
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

    public async Task<Usuario?> Create(Usuario entity)
    {
        var BuscaUsuario = await GetByUsuario(entity.NomeDeUsuario);

        if (BuscaUsuario is not null)
            return null;

        if (entity.Foto is null || entity.Foto == "")
            entity.Foto = "https://i.imgur.com/I8MfmC8.png";

        entity.Senha = BCrypt.Net.BCrypt.HashPassword(entity.Senha, workFactor: 10);

        _dbContext.Usuarios.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Usuario?> Update(Usuario entity)
    {
        var UsuarioUpdate = await _dbContext.Usuarios.FindAsync(entity.Id);

        if (UsuarioUpdate is null)
            return null;

        if (entity.Foto is null || entity.Foto == "")
            entity.Foto = "https://i.imgur.com/I8MfmC8.png";

        entity.Senha = BCrypt.Net.BCrypt.HashPassword(entity.Senha, workFactor: 10);

        _dbContext.Entry(UsuarioUpdate).State = EntityState.Detached;
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task Delete(Usuario entity)
    {
        _dbContext.Usuarios.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Usuario?> GetByUsuario(string usuario)
    {
        try
        {
            var BuscaUsuario = await _dbContext.Usuarios
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
}
