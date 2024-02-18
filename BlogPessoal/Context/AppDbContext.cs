using BlogPessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace BlogPessoal.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    // Registro das Entidades
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Tema> Temas { get; set; }
    public DbSet<Publicacao> Publicacoes { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("tb_usuario");
        modelBuilder.Entity<Tema>().ToTable("tb_tema");
        modelBuilder.Entity<Publicacao>().ToTable("tb_publicacao");
        modelBuilder.Entity<Comentario>().ToTable("tb_comentario");

        // Relacionamento Publicacao -> Tema
        modelBuilder.Entity<Publicacao>()
            .HasOne(p => p.Tema)
            .WithMany(t => t.Publicacao)
            .HasForeignKey("TemaId")
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento Publicacao -> Usuario
        modelBuilder.Entity<Publicacao>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Publicacao)
            .HasForeignKey("UsuarioId")
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento Comentario -> Publicacao
        modelBuilder.Entity<Comentario>()
            .HasOne(p => p.Publicacao)
            .WithMany(u => u.Comentario)
            .HasForeignKey("PublicacaoId")
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento Comentario -> Usuario
        modelBuilder.Entity<Comentario>()
            .HasOne(p => p.Usuario)
            .WithMany(t => t.Comentario)
            .HasForeignKey("UsuarioId")
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntries = ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);

        foreach (var insertedEntry in insertedEntries)
        {
            //Se uma propriedade da Classe Auditable estiver sendo criada. 
            if (insertedEntry is Auditable auditableEntity)
            {
                auditableEntity.Data = DateTimeOffset.Now;
            }
        }

        var modifiedEntries = ChangeTracker.Entries()
                   .Where(x => x.State == EntityState.Modified)
                   .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries)
        {
            //Se uma propriedade da Classe Auditable estiver sendo atualizada.  
            if (modifiedEntry is Auditable auditableEntity)
            {
                auditableEntity.Data = DateTimeOffset.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
