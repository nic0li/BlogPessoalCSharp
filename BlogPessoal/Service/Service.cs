using BlogPessoal.Service.Interfaces;

namespace BlogPessoal.Service;

public class Service<T> : IService<T> where T : class
{
    public Task<IEnumerable<T>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<T> Create(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T?> Update(T entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(T entity)
    {
        throw new NotImplementedException();
    }
}
