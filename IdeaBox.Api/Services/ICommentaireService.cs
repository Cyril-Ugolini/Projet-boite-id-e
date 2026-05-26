using IdeaBox.Api.DTOs;

namespace IdeaBox.Api.Services;

/// <summary>
/// Interface du service de gestion des commentaires.
/// Définit le contrat entre les controllers et la logique métier.
/// </summary>
public interface ICommentaireService
{
    /// <summary>Ajoute un commentaire sur une idée.</summary>
    Task<CommentaireDto?> CreateAsync(int ideeId, CreateCommentaireDto dto);

    /// <summary>Met à jour le contenu d'un commentaire. Retourne false si introuvable.</summary>
    Task<bool> UpdateAsync(int ideeId, int commentaireId, UpdateCommentaireDto dto);

    /// <summary>Supprime un commentaire. Retourne false si introuvable.</summary>
    Task<bool> DeleteAsync(int ideeId, int commentaireId);
}