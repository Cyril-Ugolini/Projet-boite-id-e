using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Services;

/**
 * Implémentation du service de gestion des commentaires.
 * Contient la logique métier et les accès à la base de données via EF Core.
 */
public class CommentaireService : ICommentaireService
{
    // Contexte EF Core pour accéder à la base de données
    private readonly AppDbContext _context;

    // Logger pour tracer les actions et erreurs
    private readonly ILogger<CommentaireService> _logger;

    // Injection des dépendances via le constructeur
    public CommentaireService(AppDbContext context, ILogger<CommentaireService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Création d'un commentaire pour une idée
    public async Task<CommentaireDto?> CreateAsync(int ideeId, CreateCommentaireDto dto)
    {
        _logger.LogInformation("Ajout d'un commentaire sur l'idée {IdeeId} par {Auteur}", ideeId, dto.Auteur);

        // Vérifie que l'idée existe
        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour ajout commentaire", ideeId);
            return null;
        }

        // Création de l'entité Commentaire à partir du DTO (mapping)
        var commentaire = new Commentaire
        {
            Contenu      = dto.Contenu,
            Auteur       = dto.Auteur,
            IdIdee       = ideeId,
            DateCreation = DateTime.UtcNow
        };

        // Ajout en base
        _context.Commentaires.Add(commentaire);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {IdCommentaire} créé avec succès", commentaire.IdCommentaire);

        // Mapping entité → DTO pour retour API
        return new CommentaireDto
        {
            IdCommentaire = commentaire.IdCommentaire,
            Contenu       = commentaire.Contenu,
            Auteur        = commentaire.Auteur,
            DateCreation  = commentaire.DateCreation
        };
    }

    // Mise à jour d'un commentaire existant
    public async Task<bool> UpdateAsync(int ideeId, int commentaireId, UpdateCommentaireDto dto)
    {
        _logger.LogInformation("Mise à jour du commentaire {CommentaireId} sur l'idée {IdeeId}", commentaireId, ideeId);

        // Recherche du commentaire correspondant à l'idée
        var commentaire = await _context.Commentaires
            .FirstOrDefaultAsync(c => c.IdCommentaire == commentaireId && c.IdIdee == ideeId);

        if (commentaire is null)
        {
            _logger.LogWarning("Commentaire {CommentaireId} introuvable pour mise à jour", commentaireId);
            return false;
        }

        // Mise à jour des données
        commentaire.Contenu = dto.Contenu;

        // Sauvegarde
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {CommentaireId} mis à jour avec succès", commentaireId);
        return true;
    }

    // Suppression d'un commentaire
    public async Task<bool> DeleteAsync(int ideeId, int commentaireId)
    {
        _logger.LogInformation("Suppression du commentaire {CommentaireId} sur l'idée {IdeeId}", commentaireId, ideeId);

        // Recherche du commentaire
        var commentaire = await _context.Commentaires
            .FirstOrDefaultAsync(c => c.IdCommentaire == commentaireId && c.IdIdee == ideeId);

        if (commentaire is null)
        {
            _logger.LogWarning("Commentaire {CommentaireId} introuvable", commentaireId);
            return false;
        }

        // Suppression
        _context.Commentaires.Remove(commentaire);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {CommentaireId} supprimé avec succès", commentaireId);
        return true;
    }
}
