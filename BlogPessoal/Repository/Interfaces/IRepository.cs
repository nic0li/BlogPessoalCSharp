namespace BlogPessoal.Repository.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();

    Task<T?> GetById(long id);

    Task<T?> Create(T entity);

    Task<T?> Update(T entity);

    Task Delete(T entity);
}
