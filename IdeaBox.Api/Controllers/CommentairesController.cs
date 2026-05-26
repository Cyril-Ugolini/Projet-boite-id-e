using Microsoft.AspNetCore.Mvc;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Services;

namespace IdeaBox.Api.Controllers;

/// <summary>
/// Controller REST pour la gestion des commentaires.
/// Délègue la logique métier à ICommentaireService.
/// </summary>
[ApiController]
[Route("api/idees/{ideeId}/commentaires")]
public class CommentairesController : ControllerBase
{
    private readonly ICommentaireService _commentaireService;
    private readonly ILogger<CommentairesController> _logger;

    /// <summary>
    /// Constructeur — injection du service et du logger.
    /// </summary>
    public CommentairesController(ICommentaireService commentaireService, ILogger<CommentairesController> logger)
    {
        _commentaireService = commentaireService;
        _logger = logger;
    }

    /// <summary>Ajoute un commentaire sur une idée.</summary>
    /// <returns>201 Created | 404 Not Found</returns>
    [HttpPost]
    public async Task<ActionResult<CommentaireDto>> Create(int ideeId, CreateCommentaireDto dto)
    {
        var commentaire = await _commentaireService.CreateAsync(ideeId, dto);
        if (commentaire is null)
            return NotFound($"Idée {ideeId} introuvable.");

        return CreatedAtAction(nameof(Create), new { ideeId }, commentaire);
    }

    /// <summary>Met à jour un commentaire existant.</summary>
    /// <returns>204 No Content | 404 Not Found</returns>
    [HttpPut("{commentaireId}")]
    public async Task<IActionResult> Update(int ideeId, int commentaireId, UpdateCommentaireDto dto)
    {
        var succes = await _commentaireService.UpdateAsync(ideeId, commentaireId, dto);
        if (!succes)
            return NotFound();

        return NoContent();
    }

    /// <summary>Supprime un commentaire.</summary>
    /// <returns>204 No Content | 404 Not Found</returns>
    [HttpDelete("{commentaireId}")]
    public async Task<IActionResult> Delete(int ideeId, int commentaireId)
    {
        var succes = await _commentaireService.DeleteAsync(ideeId, commentaireId);
        if (!succes)
            return NotFound();

        return NoContent();
    }
}