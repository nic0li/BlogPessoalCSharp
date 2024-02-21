using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Repository;

public class TemaRepository : ITemaRepository
{
    private AppDbContext _dbContext;

    public TemaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Tema>> GetAll()
    {
        return await _dbContext.Temas
             .Include(t => t.Publicacao)
             .ToListAsync();
    }

    public async Task<Tema?> GetById(long id)
    {
        try
        {
            var Tema = await _dbContext.Temas
                 .Include(t => t.Publicacao)
                 .FirstAsync(t => t.Id == id);

            return Tema;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Tema?> Create(Tema entity)
    {
        _dbContext.Temas.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Tema?> Update(Tema entity)
    {
        var TemaUpdate = await _dbContext.Temas.FindAsync(entity.Id);

        if (TemaUpdate == null)
            return null;

        _dbContext.Entry(TemaUpdate).State = EntityState.Detached;
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task Delete(Tema entity)
    {
        _dbContext.Temas.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tema>> GetByDescricao(string descricao)
    {
        var Tema = await _dbContext.Temas
            .Include(t => t.Publicacao)
            .Where(t => t.Descricao.Contains(descricao))
            .ToListAsync();

        return Tema;
    }
}
