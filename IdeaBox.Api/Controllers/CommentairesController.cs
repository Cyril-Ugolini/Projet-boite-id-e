using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Controllers;

[ApiController]
[Route("api/idees/{ideeId}/commentaires")]
public class CommentairesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CommentairesController> _logger;

    public CommentairesController(AppDbContext context, ILogger<CommentairesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // POST api/idees/5/commentaires
    [HttpPost]
    public async Task<ActionResult<CommentaireDto>> Create(int ideeId, CreateCommentaireDto dto)
    {
        _logger.LogInformation("Ajout d'un commentaire sur l'idée {IdeeId} par {Auteur}", ideeId, dto.Auteur);

        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour ajout commentaire", ideeId);
            return NotFound($"Idée {ideeId} introuvable.");
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

        var result = new CommentaireDto
        {
            IdCommentaire = commentaire.IdCommentaire,
            Contenu       = commentaire.Contenu,
            Auteur        = commentaire.Auteur,
            DateCreation  = commentaire.DateCreation
        };

        return CreatedAtAction(nameof(Create), new { ideeId }, result);
    }

    // DELETE api/idees/5/commentaires/3
    [HttpDelete("{commentaireId}")]
    public async Task<IActionResult> Delete(int ideeId, int commentaireId)
    {
        _logger.LogInformation("Suppression du commentaire {CommentaireId} sur l'idée {IdeeId}", commentaireId, ideeId);

        var commentaire = await _context.Commentaires
            .FirstOrDefaultAsync(c => c.IdCommentaire == commentaireId && c.IdIdee == ideeId);

        if (commentaire is null)
        {
            _logger.LogWarning("Commentaire {CommentaireId} introuvable", commentaireId);
            return NotFound();
        }

        _context.Commentaires.Remove(commentaire);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Commentaire {CommentaireId} supprimé avec succès", commentaireId);

        return NoContent();
    }
}