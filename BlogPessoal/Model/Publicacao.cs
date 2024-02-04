using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogPessoal.Model;

public class Publicacao : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Column(TypeName = "varchar")]
    [StringLength(1000)]
    public string Texto { get; set; } = string.Empty;

    public virtual Tema? Tema { get; set; }

    public virtual Usuario? Usuario { get; set; }

    [InverseProperty("Publicacao")]
    public virtual ICollection<Comentario>? Comentario { get; set; }
}
