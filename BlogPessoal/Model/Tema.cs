using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogPessoal.Model;

public class Tema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Descricao { get; set; } = string.Empty;

    [InverseProperty("Tema")]
    public virtual ICollection<Publicacao>? Publicacao { get; set; }
}
