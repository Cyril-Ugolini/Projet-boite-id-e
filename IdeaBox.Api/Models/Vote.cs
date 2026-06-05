using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeaBox.Api.Models;

// Entité représentant un vote associé à une idée
[Table("vote")]
public class Vote
{
    // Clé primaire du vote
    [Key]
    [Column("id_vote")]
    public int IdVote { get; set; }

    // Nom ou pseudo de l'utilisateur ayant voté
    // Obligatoire, limité à 100 caractères
    [Column("auteur")]
    [Required]
    [MaxLength(100)]
    public string Auteur { get; set; } = string.Empty;

    // Date de création du vote
    // Initialisée automatiquement avec la date actuelle (UTC)
    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Clé étrangère vers l'idée associée
    [Column("id_idee")]
    public int IdIdee { get; set; }

    // Propriété de navigation vers l'entité Idee
    // Permet d'accéder à l'idée liée à ce vote
    [ForeignKey("IdIdee")]
    public Idee? Idee { get; set; }
}