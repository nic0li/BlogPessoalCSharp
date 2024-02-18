using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;

namespace BlogPessoal.Repository;

public class TemaRepository : Repository<Tema>, ITemaRepository
{
    public TemaRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
