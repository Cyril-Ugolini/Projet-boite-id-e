using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Services;

/*
 * Service métier pour la gestion des idées.
 * 
 * Responsabilités :
 * - Accès aux données via EF Core (DbContext)
 * - Application de la logique métier
 * - Mapping entre les entités (Models) et les DTOs
 * - Journalisation (logs)
 */
public class IdeeService : IIdeeService
{
    // Contexte de base de données (Entity Framework Core)
    private readonly AppDbContext _context;

    // Logger pour suivre les actions et erreurs
    private readonly ILogger<IdeeService> _logger;

    // Injection des dépendances (DbContext + Logger)
    public IdeeService(AppDbContext context, ILogger<IdeeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /*
     * Récupère toutes les idées (version liste simplifiée)
     * 
     * Retourne une liste de IdeeListDto contenant uniquement
     * les informations nécessaires pour un affichage résumé.
     */
    public async Task<List<IdeeListDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de toutes les idées");

        return await _context.Idees
            .Include(i => i.Commentaires) // Chargement des commentaires
            .Include(i => i.Votes)        // Chargement des votes
            .OrderByDescending(i => i.DateCreation) // Tri par date (plus récent en premier)
            .Select(i => new IdeeListDto
            {
                IdIdee         = i.IdIdee,
                Titre          = i.Titre,
                Auteur         = i.Auteur,
                Priorite       = i.Priorite,
                Difficulte     = i.Difficulte,
                NbCommentaires = i.Commentaires.Count, // Calcul du nombre de commentaires
                NbVotes        = i.Votes.Count,        // Calcul du nombre de votes
                DateCreation   = i.DateCreation
            })
            .ToListAsync();
    }

    /*
     * Récupère une idée par son identifiant (vue détaillée)
     * 
     * Inclut :
     * - toutes les informations de l'idée
     * - liste des commentaires
     * - nombre de votes
     */
    public async Task<IdeeDetailDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de l'idée {Id}", id);

        // Recherche avec chargement des relations
        var idee = await _context.Idees
            .Include(i => i.Commentaires)
            .Include(i => i.Votes)
            .FirstOrDefaultAsync(i => i.IdIdee == id);

        if (idee is null)
        {
            _logger.LogWarning("Idée {Id} introuvable", id);
            return null;
        }

        // Mapping entité → DTO détaillé
        return new IdeeDetailDto
        {
            IdIdee           = idee.IdIdee,
            Titre            = idee.Titre,
            Contenu          = idee.Contenu,
            Auteur           = idee.Auteur,
            Priorite         = idee.Priorite,
            Difficulte       = idee.Difficulte,
            DateCreation     = idee.DateCreation,
            DateModification = idee.DateModification,
            NbVotes          = idee.Votes.Count,

            // Transformation des commentaires en DTO
            Commentaires = idee.Commentaires
                .OrderBy(c => c.DateCreation)
                .Select(c => new CommentaireDto
                {
                    IdCommentaire = c.IdCommentaire,
                    Contenu       = c.Contenu,
                    Auteur        = c.Auteur,
                    DateCreation  = c.DateCreation
                })
                .ToList()
        };
    }

    /*
     * Crée une nouvelle idée
     * 
     * - Convertit le DTO en entité
     * - Sauvegarde en base
     * - Retourne l'idée complète (DTO détail)
     */
    public async Task<IdeeDetailDto> CreateAsync(CreateIdeeDto dto)
    {
        _logger.LogInformation("Création d'une idée par {Auteur}", dto.Auteur);

        // Mapping DTO → entité
        var idee = new Idee
        {
            Titre            = dto.Titre,
            Contenu          = dto.Contenu,
            Auteur           = dto.Auteur,
            Priorite         = dto.Priorite,
            Difficulte       = dto.Difficulte,
            DateCreation     = DateTime.UtcNow,
            DateModification = DateTime.UtcNow
        };

        _context.Idees.Add(idee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Idée {Id} créée avec succès", idee.IdIdee);

        // On retourne l'objet complet en réutilisant la méthode existante
        return (await GetByIdAsync(idee.IdIdee))!;
    }

    /*
     * Met à jour une idée existante
     * 
     * Retour :
     * - true  → succès
     * - false → idée introuvable
     */
    public async Task<bool> UpdateAsync(int id, UpdateIdeeDto dto)
    {
        var idee = await _context.Idees.FindAsync(id);

        if (idee is null)
        {
            _logger.LogWarning("Idée {Id} introuvable pour mise à jour", id);
            return false;
        }

        // Mise à jour des champs modifiables
        idee.Titre            = dto.Titre;
        idee.Contenu          = dto.Contenu;
        idee.Priorite         = dto.Priorite;
        idee.Difficulte       = dto.Difficulte;
        idee.DateModification = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Idée {Id} mise à jour avec succès", id);
        return true;
    }

    /*
     * Supprime une idée
     * 
     * Retour :
     * - true  → succès
     * - false → idée introuvable
     */
    public async Task<bool> DeleteAsync(int id)
    {
        var idee = await _context.Idees.FindAsync(id);

        if (idee is null)
        {
            _logger.LogWarning("Idée {Id} introuvable pour suppression", id);
            return false;
        }

        _context.Idees.Remove(idee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Idée {Id} supprimée avec succès", id);
        return true;
    }
}