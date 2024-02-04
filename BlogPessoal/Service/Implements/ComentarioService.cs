using BlogPessoal.Data;
using BlogPessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Service.Implements;

public class ComentarioService : IComentarioService
{
    private readonly AppDbContext _context;

    public ComentarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comentario>> GetAll()
    {
        return await _context.Comentarios
            .Include(p => p.Publicacao)
            .Include(p => p.Usuario)
            .ToListAsync();
    }

    public async Task<Comentario?> GetById(long id)
    {
        try
        {
            var comentario = await _context.Comentarios
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

    public async Task<IEnumerable<Comentario>> GetByTexto(string texto)
    {
        var comentario = await _context.Comentarios
            .Include(p => p.Publicacao)
            .Include(p => p.Usuario)
            .Where(p => p.Texto.Contains(texto))
            .ToListAsync();

        return comentario;
    }

    public async Task<Comentario?> Create(Comentario comentario)
    {
        if (comentario.Publicacao is not null)
        {
            var buscaPublicacao = await _context.Publicacoes.FindAsync(comentario.Publicacao.Id);

            if (buscaPublicacao is null)
                return null;

            comentario.Publicacao = buscaPublicacao;
        }

        comentario.Usuario = comentario.Usuario is not null ? await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == comentario.Usuario.Id) : null;

        await _context.Comentarios.AddAsync(comentario);
        await _context.SaveChangesAsync();

        return comentario;
    }

    public async Task<Comentario?> Update(Comentario comentario)
    {
        var atualizaPublicacao = await _context.Comentarios.FindAsync(comentario.Id);

        if (atualizaPublicacao is null)
            return null;

        if (comentario.Publicacao is not null)
        {
            var buscaPublicacao = await _context.Publicacoes.FindAsync(comentario.Publicacao.Id);

            if (buscaPublicacao is null)
                return null;

            comentario.Publicacao = buscaPublicacao;

        }

        comentario.Usuario = comentario.Usuario is not null ? await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == comentario.Usuario.Id) : null;

        _context.Entry(atualizaPublicacao).State = EntityState.Detached;
        _context.Entry(comentario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return comentario;
    }

    public async Task Delete(Comentario comentario)
    {
        _context.Comentarios.Remove(comentario);
        await _context.SaveChangesAsync();
    }
}
