using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeaBox.Api.Models;

// Entité représentant un commentaire en base de données
[Table("commentaire")]
public class Commentaire
{
    // Clé primaire du commentaire
    [Key]
    [Column("id_commentaire")]
    public int IdCommentaire { get; set; }

    // Contenu textuel du commentaire
    // Obligatoire
    [Column("contenu")]
    [Required]
    public string Contenu { get; set; } = string.Empty;

    // Nom ou pseudo de l'auteur du commentaire
    // Obligatoire et limité à 100 caractères
    [Column("auteur")]
    [Required]
    [MaxLength(100)]
    public string Auteur { get; set; } = string.Empty;

    // Date de création du commentaire
    // Initialisée automatiquement à la date actuelle (UTC)
    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Clé étrangère vers l'idée associée
    [Column("id_idee")]
    public int IdIdee { get; set; }

    // Propriété de navigation vers l'entité Idee
    // Permet d'accéder à l'idée liée au commentaire
    [ForeignKey("IdIdee")]
    public Idee? Idee { get; set; }
}