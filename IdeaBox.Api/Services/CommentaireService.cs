using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Services;

/// <summary>
/// Implémentation du service de gestion des commentaires.
/// Contient la logique métier et les accès à la base de données via EF Core.
/// </summary>
public class CommentaireService : ICommentaireService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CommentaireService> _logger;

    public CommentaireService(AppDbContext context, ILogger<CommentaireService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CommentaireDto?> CreateAsync(int ideeId, CreateCommentaireDto dto)
    {
        _logger.LogInformation("Ajout d'un commentaire sur l'idée {IdeeId} par {Auteur}", ideeId, dto.Auteur);

        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour ajout commentaire", ideeId);
            return null;
        }

        var commentaire = new Commentaire
        {
            Contenu      = dto.Contenu,
            Auteur       = dto.Auteur,
            IdIdee       = ideeId,
            DateCreation = DateTime.UtcNow
        };

        _context.Commentaires.Add(commentaire);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {IdCommentaire} créé avec succès", commentaire.IdCommentaire);

        return new CommentaireDto
        {
            IdCommentaire = commentaire.IdCommentaire,
            Contenu       = commentaire.Contenu,
            Auteur        = commentaire.Auteur,
            DateCreation  = commentaire.DateCreation
        };
    }

    public async Task<bool> UpdateAsync(int ideeId, int commentaireId, UpdateCommentaireDto dto)
    {
        _logger.LogInformation("Mise à jour du commentaire {CommentaireId} sur l'idée {IdeeId}", commentaireId, ideeId);

        var commentaire = await _context.Commentaires
            .FirstOrDefaultAsync(c => c.IdCommentaire == commentaireId && c.IdIdee == ideeId);

        if (commentaire is null)
        {
            _logger.LogWarning("Commentaire {CommentaireId} introuvable pour mise à jour", commentaireId);
            return false;
        }

        commentaire.Contenu = dto.Contenu;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {CommentaireId} mis à jour avec succès", commentaireId);
        return true;
    }

    public async Task<bool> DeleteAsync(int ideeId, int commentaireId)
    {
        _logger.LogInformation("Suppression du commentaire {CommentaireId} sur l'idée {IdeeId}", commentaireId, ideeId);

        var commentaire = await _context.Commentaires
            .FirstOrDefaultAsync(c => c.IdCommentaire == commentaireId && c.IdIdee == ideeId);

        if (commentaire is null)
        {
            _logger.LogWarning("Commentaire {CommentaireId} introuvable", commentaireId);
            return false;
        }

        _context.Commentaires.Remove(commentaire);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {CommentaireId} supprimé avec succès", commentaireId);
        return true;
    }
}