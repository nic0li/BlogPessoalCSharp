using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogPessoal.Model;

public class Comentario : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(1000)]
    public string Texto { get; set; } = string.Empty;

    public virtual Usuario? Usuario { get; set; }

    public virtual Publicacao? Publicacao { get; set; }
}
