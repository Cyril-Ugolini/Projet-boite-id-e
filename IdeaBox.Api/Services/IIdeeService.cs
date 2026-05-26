using IdeaBox.Api.DTOs;

namespace IdeaBox.Api.Services;

/// <summary>
/// Interface du service de gestion des idées.
/// Définit le contrat entre les controllers et la logique métier.
/// </summary>
public interface IIdeeService
{
    /// <summary>Récupère la liste de toutes les idées.</summary>
    Task<List<IdeeListDto>> GetAllAsync();

    /// <summary>Récupère le détail d'une idée par son id.</summary>
    Task<IdeeDetailDto?> GetByIdAsync(int id);

    /// <summary>Crée une nouvelle idée.</summary>
    Task<IdeeDetailDto> CreateAsync(CreateIdeeDto dto);

    /// <summary>Met à jour une idée existante.</summary>
    Task<bool> UpdateAsync(int id, UpdateIdeeDto dto);

    /// <summary>Supprime une idée et ses commentaires/votes en cascade.</summary>
    Task<bool> DeleteAsync(int id);
}