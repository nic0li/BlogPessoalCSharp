using BlogPessoal.Context;
using BlogPessoal.Model;
using BlogPessoal.Repository.Interfaces;

namespace BlogPessoal.Repository;

public class ComentarioRepository : Repository<Comentario>, IComentarioRepository
{
    public ComentarioRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
