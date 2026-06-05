using IdeaBox.Api.DTOs;

namespace IdeaBox.Api.Services;

/**
* Interface du service de gestion des commentaires.
* Définit le contrat entre les controllers et la logique métier.
*/
public interface ICommentaireService
{
    // Ajoute un commentaire sur une idée.
    Task<CommentaireDto?> CreateAsync(int ideeId, CreateCommentaireDto dto);

    // Met à jour le contenu d'un commentaire. Retourne false si introuvable.
    Task<bool> UpdateAsync(int ideeId, int commentaireId, UpdateCommentaireDto dto);

    // Supprime un commentaire. Retourne false si introuvable.
    Task<bool> DeleteAsync(int ideeId, int commentaireId);
}