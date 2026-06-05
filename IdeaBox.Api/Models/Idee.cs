using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeaBox.Api.Models;

// Entité représentant une idée dans la base de données
[Table("idee")]
public class Idee
{
    // Clé primaire de l'idée
    [Key]
    [Column("id_idee")]
    public int IdIdee { get; set; }

    // Titre de l'idée
    // Obligatoire et limité à 50 caractères
    [Column("titre")]
    [Required]
    [MaxLength(50)]
    public string Titre { get; set; } = string.Empty;

    // Description complète de l'idée
    // Obligatoire
    [Column("contenu")]
    [Required]
    public string Contenu { get; set; } = string.Empty;

    // Nom ou pseudo de l'auteur
    // Obligatoire et limité à 100 caractères
    [Column("auteur")]
    [Required]
    [MaxLength(100)]
    public string Auteur { get; set; } = string.Empty;

    // Priorité de l'idée (ex: faible, moyenne, haute)
    // Obligatoire avec une valeur par défaut
    [Column("priorite")]
    [Required]
    [MaxLength(10)]
    public string Priorite { get; set; } = "moyenne";

    // Niveau de difficulté de réalisation
    // Obligatoire avec une valeur par défaut
    [Column("difficulte")]
    [Required]
    [MaxLength(10)]
    public string Difficulte { get; set; } = "moyenne";

    // Date de création de l'idée
    // Initialisée automatiquement à la date UTC actuelle
    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Date de dernière modification
    // Mise à jour lors d'une modification
    [Column("date_modification")]
    public DateTime DateModification { get; set; } = DateTime.UtcNow;

    // Navigation vers les commentaires liés à l'idée
    // Permet de récupérer tous les commentaires associés
    public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();

    // Navigation vers les votes liés à l'idée
    // Permet de récupérer tous les votes associés
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}