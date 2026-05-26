using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Services;

/// <summary>
/// Implémentation du service de gestion des idées.
/// Contient la logique métier et les accès à la base de données via EF Core.
/// </summary>
public class IdeeService : IIdeeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<IdeeService> _logger;

    public IdeeService(AppDbContext context, ILogger<IdeeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<IdeeListDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de toutes les idées");

        return await _context.Idees
            .Include(i => i.Commentaires)
            .Include(i => i.Votes)
            .OrderByDescending(i => i.DateCreation)
            .Select(i => new IdeeListDto
            {
                IdIdee         = i.IdIdee,
                Titre          = i.Titre,
                Auteur         = i.Auteur,
                Priorite       = i.Priorite,
                Difficulte     = i.Difficulte,
                NbCommentaires = i.Commentaires.Count,
                NbVotes        = i.Votes.Count,
                DateCreation   = i.DateCreation
            })
            .ToListAsync();
    }

    public async Task<IdeeDetailDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de l'idée {Id}", id);

        var idee = await _context.Idees
            .Include(i => i.Commentaires)
            .Include(i => i.Votes)
            .FirstOrDefaultAsync(i => i.IdIdee == id);

        if (idee is null)
        {
            _logger.LogWarning("Idée {Id} introuvable", id);
            return null;
        }

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
            Commentaires     = idee.Commentaires
                .OrderBy(c => c.DateCreation)
                .Select(c => new CommentaireDto
                {
                    IdCommentaire = c.IdCommentaire,
                    Contenu       = c.Contenu,
                    Auteur        = c.Auteur,
                    DateCreation  = c.DateCreation
                }).ToList()
        };
    }

    public async Task<IdeeDetailDto> CreateAsync(CreateIdeeDto dto)
    {
        _logger.LogInformation("Création d'une idée par {Auteur}", dto.Auteur);

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

        return (await GetByIdAsync(idee.IdIdee))!;
    }

    public async Task<bool> UpdateAsync(int id, UpdateIdeeDto dto)
    {
        var idee = await _context.Idees.FindAsync(id);
        if (idee is null)
        {
            _logger.LogWarning("Idée {Id} introuvable pour mise à jour", id);
            return false;
        }

        idee.Titre            = dto.Titre;
        idee.Contenu          = dto.Contenu;
        idee.Priorite         = dto.Priorite;
        idee.Difficulte       = dto.Difficulte;
        idee.DateModification = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Idée {Id} mise à jour avec succès", id);
        return true;
    }

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