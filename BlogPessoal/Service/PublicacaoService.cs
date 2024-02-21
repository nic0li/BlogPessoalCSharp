using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using BlogPessoal.Service.Interfaces;

namespace BlogPessoal.Service;

public class PublicacaoService : IPublicacaoService
{
    private readonly IPublicacaoRepository _repository;

    public PublicacaoService(IPublicacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Publicacao>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Publicacao?> GetById(long id)
    {
        return await _repository.GetById(id);
    }

    public async Task<Publicacao?> Create(Publicacao entity)
    {
        return await _repository.Create(entity);
    }

    public async Task<Publicacao?> Update(Publicacao entity)
    {
        return await _repository.Update(entity);
    }

    public async Task Delete(Publicacao entity)
    {
        await _repository.Delete(entity);
    }

    public async Task<IEnumerable<Publicacao?>> GetByTitulo(string titulo)
    {
        return await _repository.GetByTitulo(titulo);
    }
}
