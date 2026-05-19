using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeaBox.Api.Models;

[Table("idee")]
public class Idee
{
    [Key]
    [Column("id_idee")]
    public int IdIdee { get; set; }

    [Column("titre")]
    [Required]
    [MaxLength(50)]
    public string Titre { get; set; } = string.Empty;

    [Column("contenu")]
    [Required]
    public string Contenu { get; set; } = string.Empty;

    [Column("auteur")]
    [Required]
    [MaxLength(100)]
    public string Auteur { get; set; } = string.Empty;

    [Column("priorite")]
    [Required]
    [MaxLength(10)]
    public string Priorite { get; set; } = "moyenne";

    [Column("difficulte")]
    [Required]
    [MaxLength(10)]
    public string Difficulte { get; set; } = "moyenne";

    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    [Column("date_modification")]
    public DateTime DateModification { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}