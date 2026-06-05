using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Services;

namespace IdeaBox.Api.Controllers;

 /**
 *Controller REST pour la gestion des commentaires.
 *Délègue la logique métier à ICommentaireService.
 *POST accessible sans authentification.
 *PUT et DELETE réservés au rôle dev.
 */
[ApiController]
[Route("api/idees/{ideeId}/commentaires")]
public class CommentairesController : ControllerBase
{
    private readonly ICommentaireService _commentaireService;
    private readonly ILogger<CommentairesController> _logger;

    /**
    * Constructeur — injection du service et du logger.
    */
    public CommentairesController(ICommentaireService commentaireService, ILogger<CommentairesController> logger)
    {
        _commentaireService = commentaireService;
        _logger = logger;
    }

    /**Ajoute un commentaire sur une idée.</summary>
    * <returns>201 Created | 404 Not Found</returns>
    */
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<CommentaireDto>> Create(int ideeId, CreateCommentaireDto dto)
    {
        var commentaire = await _commentaireService.CreateAsync(ideeId, dto);
        if (commentaire is null)
            return NotFound($"Idée {ideeId} introuvable.");

        return CreatedAtAction(nameof(Create), new { ideeId }, commentaire);
    }

    /**Met à jour un commentaire existant. Réservé au rôle dev.</summary>
    * <returns>204 No Content | 404 Not Found</returns>
    */
    [Authorize(Roles = "dev")]
    [HttpPut("{commentaireId}")]
    public async Task<IActionResult> Update(int ideeId, int commentaireId, UpdateCommentaireDto dto)
    {
        var succes = await _commentaireService.UpdateAsync(ideeId, commentaireId, dto);
        if (!succes)
            return NotFound();

        return NoContent();
    }

   /**Supprime un commentaire. Réservé au rôle dev.</summary>
    * <returns>204 No Content | 404 Not Found</returns>
    */
    [Authorize(Roles = "dev")]
    [HttpDelete("{commentaireId}")]
    public async Task<IActionResult> Delete(int ideeId, int commentaireId)
    {
        var succes = await _commentaireService.DeleteAsync(ideeId, commentaireId);
        if (!succes)
            return NotFound();

        return NoContent();
    }
}