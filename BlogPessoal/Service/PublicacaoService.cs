using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using BlogPessoal.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Service;

public class PublicacaoService : IPublicacaoService
{
    private readonly AppDbContext _context;
    private readonly IPublicacaoRepository _repository;

    public PublicacaoService(AppDbContext context, IPublicacaoRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<IEnumerable<Publicacao>> GetAll()
    {
        return await _context.Publicacoes
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .ToListAsync();
    }

    public async Task<Publicacao?> GetById(long id)
    {
        try
        {
            var publicacao = await _context.Publicacoes
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

    public async Task<IEnumerable<Publicacao>> GetByTitulo(string titulo)
    {
        var publicacao = await _context.Publicacoes
            .Include(p => p.Tema)
            .Include(p => p.Usuario)
            .Where(p => p.Titulo.Contains(titulo))
            .ToListAsync();

        return publicacao;
    }

    public async Task<Publicacao?> Create(Publicacao publicacao)
    {
        if (publicacao.Tema is not null)
        {
            var buscaTema = await _context.Temas.FindAsync(publicacao.Tema.Id);

            if (buscaTema is null)
                return null;

            publicacao.Tema = buscaTema;
        }

        publicacao.Usuario = publicacao.Usuario is not null ? await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == publicacao.Usuario.Id) : null;

        await _context.Publicacoes.AddAsync(publicacao);
        await _context.SaveChangesAsync();

        return publicacao;
    }

    public async Task<Publicacao?> Update(Publicacao publicacao)
    {
        var atualizaPublicacao = await _context.Publicacoes.FindAsync(publicacao.Id);

        if (atualizaPublicacao is null)
            return null;

        if (publicacao.Tema is not null)
        {
            var buscaTema = await _context.Temas.FindAsync(publicacao.Tema.Id);

            if (buscaTema is null)
                return null;

            publicacao.Tema = buscaTema;

        }

        publicacao.Usuario = publicacao.Usuario is not null ? await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == publicacao.Usuario.Id) : null;

        _context.Entry(atualizaPublicacao).State = EntityState.Detached;
        _context.Entry(publicacao).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return publicacao;
    }

    public async Task Delete(Publicacao publicacao)
    {
        _context.Publicacoes.Remove(publicacao);
        await _context.SaveChangesAsync();
    }
}
