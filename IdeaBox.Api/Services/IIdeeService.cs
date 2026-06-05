using IdeaBox.Api.DTOs;

namespace IdeaBox.Api.Services;

/**
* Interface du service de gestion des idées.
* Définit le contrat entre les controllers et la logique métier.
*/
public interface IIdeeService
{
    //Récupère la liste de toutes les idées.
    Task<List<IdeeListDto>> GetAllAsync();

    //Récupère le détail d'une idée par son id.
    Task<IdeeDetailDto?> GetByIdAsync(int id);

    //Crée une nouvelle idée.
    Task<IdeeDetailDto> CreateAsync(CreateIdeeDto dto);

    //Met à jour une idée existante.</summary>
    Task<bool> UpdateAsync(int id, UpdateIdeeDto dto);

    //Supprime une idée et ses commentaires/votes en cascade.
    Task<bool> DeleteAsync(int id);
}