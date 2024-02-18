using BlogPessoal.Context;
using BlogPessoal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogPessoal.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> GetAll()
    {
        return _dbContext.Set<T>().AsNoTracking();
    }

    public T? GetById(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.Set<T>().AsNoTracking().SingleOrDefault(predicate);
    }

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbContext.Set<T>().Update(entity);
    }
}
