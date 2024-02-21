using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using BlogPessoal.Service.Interfaces;

namespace BlogPessoal.Service;

public class ComentarioService : IComentarioService
{
    private readonly IComentarioRepository _repository;

    public ComentarioService(IComentarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Comentario>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Comentario?> GetById(long id)
    {
        return await _repository.GetById(id);
    }

    public async Task<Comentario?> Create(Comentario entity)
    {
        return await _repository.Create(entity);
    }

    public async Task<Comentario?> Update(Comentario entity)
    {
        return await _repository.Update(entity);
    }

    public async Task Delete(Comentario entity)
    {
        await _repository.Delete(entity);
    }

    public async Task<IEnumerable<Comentario?>> GetByTexto(string texto)
    {
        return await _repository.GetByTexto(texto);
    }
}
