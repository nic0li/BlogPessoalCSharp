using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repository;

public class ComentarioRepository : IComentarioRepository
{
    private AppDbContext _dbContext;

    public ComentarioRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Comentario>> GetAll()
    {
        return await _dbContext.Comentarios
            .Include(p => p.Publicacao)
            .Include(p => p.Usuario)
            .ToListAsync();
    }

    public async Task<Comentario?> GetById(long id)
    {
        try
        {
            var comentario = await _dbContext.Comentarios
                .Include(p => p.Publicacao)
                .Include(p => p.Usuario)
                .FirstAsync(p => p.Id == id);

            return comentario;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Comentario?> Create(Comentario entity)
    {
        if (entity.Publicacao is not null)
        {
            var buscaPublicacao = await _dbContext.Publicacoes.FindAsync(entity.Publicacao.Id);

            if (buscaPublicacao is null)
                return null;

            entity.Publicacao = buscaPublicacao;
        }

        entity.Usuario = entity.Usuario is not null ? 
            await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == entity.Usuario.Id) : null;

        await _dbContext.Comentarios.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Comentario?> Update(Comentario entity)
    {
        var atualizaPublicacao = await _dbContext.Comentarios.FindAsync(entity.Id);

        if (atualizaPublicacao is null)
            return null;

        if (entity.Publicacao is not null)
        {
            var buscaPublicacao = await _dbContext.Publicacoes.FindAsync(entity.Publicacao.Id);

            if (buscaPublicacao is null)
                return null;

            entity.Publicacao = buscaPublicacao;

        }

        entity.Usuario = entity.Usuario is not null ? 
            await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == entity.Usuario.Id) : null;

        _dbContext.Entry(atualizaPublicacao).State = EntityState.Detached;
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task Delete(Comentario entity)
    {
        _dbContext.Comentarios.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Comentario>> GetByTexto(string texto)
    {
        var comentario = await _dbContext.Comentarios
            .Include(p => p.Publicacao)
            .Include(p => p.Usuario)
            .Where(p => p.Texto.Contains(texto))
            .ToListAsync();

        return comentario;
    }
}
