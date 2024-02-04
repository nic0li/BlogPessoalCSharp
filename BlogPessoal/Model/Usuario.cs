using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogPessoal.Model;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Nome { get; set; } = string.Empty;

    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string NomeDeUsuario { get; set; } = string.Empty;

    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Senha { get; set; } = string.Empty;

    [Column(TypeName = "varchar")]
    [StringLength(5000)]
    public string? Foto { get; set; } = string.Empty;

    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string? Tipo { get; set; } = string.Empty;

    [InverseProperty("Usuario")]
    public virtual ICollection<Publicacao>? Publicacao { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Comentario>? Comentario { get; set; }
}
