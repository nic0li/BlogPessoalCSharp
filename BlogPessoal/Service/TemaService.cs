using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using BlogPessoal.Service.Interfaces;

namespace BlogPessoal.Service;

public class TemaService : ITemaService
{
    private readonly ITemaRepository _repository;

    public TemaService(ITemaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Tema>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Tema?> GetById(long id)
    {
        return await _repository.GetById(id);
    }

    public async Task<Tema?> Create(Tema entity)
    {
        return await _repository.Create(entity);
    }

    public async Task<Tema?> Update(Tema entity)
    {
        return await _repository.Update(entity);
    }

    public async Task Delete(Tema entity)
    {
        await _repository.Delete(entity);
    }

    public async Task<IEnumerable<Tema?>> GetByDescricao(string descricao)
    {
        return await _repository.GetByDescricao(descricao);
    }
}
