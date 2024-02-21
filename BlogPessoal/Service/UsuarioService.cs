using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;
using BlogPessoal.Service.Interfaces;

namespace BlogPessoal.Service;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Usuario>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Usuario?> GetById(long id)
    {
        return await _repository.GetById(id);
    }

    public async Task<Usuario?> Create(Usuario entity)
    {
        return await _repository.Create(entity);
    }

    public async Task<Usuario?> Update(Usuario entity)
    {
        return await _repository.Update(entity);
    }

    public async Task Delete(Usuario entity)
    {
        await _repository.Delete(entity);
    }

    public async Task<Usuario?> GetByUsuario(string usuario)
    {
        return await _repository.GetByUsuario(usuario);
    }
}
