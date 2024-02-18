using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;

namespace BlogPessoal.Repository;

public class PublicacaoRepository : Repository<Publicacao>, IPublicacaoRepository
{
    public PublicacaoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
