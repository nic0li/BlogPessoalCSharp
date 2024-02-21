using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repository;

public class PublicacaoRepository : IPublicacaoRepository
{
    private AppDbContext _dbContext;

    public PublicacaoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Publicacao>> GetAll()
    {
        return await _dbContext.Publicacoes
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .ToListAsync();
    }

    public async Task<Publicacao?> GetById(long id)
    {
        try
        {
            var publicacao = await _dbContext.Publicacoes
                .Include(p => p.Tema)
                .Include(p => p.Usuario)
                .FirstAsync(p => p.Id == id);

            return publicacao;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Publicacao?> Create(Publicacao entity)
    {
        if (entity.Tema is not null)
        {
            var buscaTema = await _dbContext.Temas.FindAsync(entity.Tema.Id);

            if (buscaTema is null)
                return null;

            entity.Tema = buscaTema;
        }

        entity.Usuario = entity.Usuario is not null ? 
            await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == entity.Usuario.Id) : null;

        await _dbContext.Publicacoes.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Publicacao?> Update(Publicacao entity)
    {
        var atualizaPublicacao = await _dbContext.Publicacoes.FindAsync(entity.Id);

        if (atualizaPublicacao is null)
            return null;

        if (entity.Tema is not null)
        {
            var buscaTema = await _dbContext.Temas.FindAsync(entity.Tema.Id);

            if (buscaTema is null)
                return null;

            entity.Tema = buscaTema;

        }

        entity.Usuario = entity.Usuario is not null ? 
            await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == entity.Usuario.Id) : null;

        _dbContext.Entry(atualizaPublicacao).State = EntityState.Detached;
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task Delete(Publicacao entity)
    {
        _dbContext.Publicacoes.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Publicacao>> GetByTitulo(string titulo)
    {
        var publicacao = await _dbContext.Publicacoes
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .Where(p => p.Titulo.Contains(titulo))
            .ToListAsync();

        return publicacao;
    }
}
