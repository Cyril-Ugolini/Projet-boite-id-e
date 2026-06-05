using IdeaBox.Api.DTOs;

namespace IdeaBox.Api.Services;

/**
* Interface du service de gestion des votes.
* Définit le contrat entre les controllers et la logique métier.
*/
public interface IVoteService
{
    /**
    * Enregistre un vote pour une idée.
    * Retourne null si l'idée est introuvable.
    * Retourne -1 si l'auteur a déjà voté pour cette idée.
    * Retourne le nombre de votes total si le vote est enregistré.
    */
    Task<int?> VoterAsync(int ideeId, CreateVoteDto dto);

    /**
    * Récupère le nombre de votes d'une idée.
    * Retourne null si l'idée est introuvable.
    */
    Task<int?> GetNbVotesAsync(int ideeId);

    /**
    * Supprime le vote d'un auteur sur une idée.
    * Retourne null si l'idée est introuvable.
    * Retourne false si le vote n'existe pas.
    * Retourne true si le vote est supprimé.
    */
    Task<bool?> SupprimerVoteAsync(int ideeId, string auteur);
}