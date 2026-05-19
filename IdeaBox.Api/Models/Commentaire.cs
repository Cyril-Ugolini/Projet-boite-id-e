using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeaBox.Api.Models;

[Table("commentaire")]
public class Commentaire
{
    [Key]
    [Column("id_commentaire")]
    public int IdCommentaire { get; set; }

    [Column("contenu")]
    [Required]
    public string Contenu { get; set; } = string.Empty;

    [Column("auteur")]
    [Required]
    [MaxLength(100)]
    public string Auteur { get; set; } = string.Empty;

    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    [Column("id_idee")]
    public int IdIdee { get; set; }

    // Navigation
    [ForeignKey("IdIdee")]
    public Idee? Idee { get; set; }
}