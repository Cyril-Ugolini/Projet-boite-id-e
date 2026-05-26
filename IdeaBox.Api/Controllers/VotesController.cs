using Microsoft.AspNetCore.Mvc;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Services;

namespace IdeaBox.Api.Controllers;

/// <summary>
/// Controller REST pour la gestion des votes.
/// Délègue la logique métier à IVoteService.
/// </summary>
[ApiController]
[Route("api/idees/{ideeId}/votes")]
public class VotesController : ControllerBase
{
    private readonly IVoteService _voteService;
    private readonly ILogger<VotesController> _logger;

    /// <summary>
    /// Constructeur — injection du service et du logger.
    /// </summary>
    public VotesController(IVoteService voteService, ILogger<VotesController> logger)
    {
        _voteService = voteService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère le nombre de votes d'une idée.
    /// </summary>
    /// <returns>200 OK | 404 Not Found</returns>
    [HttpGet]
    public async Task<IActionResult> GetNbVotes(int ideeId)
    {
        var nbVotes = await _voteService.GetNbVotesAsync(ideeId);

        if (nbVotes is null)
            return NotFound($"Idée {ideeId} introuvable.");

        return Ok(new { nbVotes });
    }

    /// <summary>
    /// Enregistre un vote pour une idée.
    /// Retourne 409 Conflict si l'auteur a déjà voté.
    /// </summary>
    /// <returns>200 OK | 404 Not Found | 409 Conflict</returns>
    [HttpPost]
    public async Task<IActionResult> Vote(int ideeId, CreateVoteDto dto)
    {
        var result = await _voteService.VoterAsync(ideeId, dto);

        if (result is null)
            return NotFound($"Idée {ideeId} introuvable.");

        if (result == -1)
            return Conflict("Vous avez déjà voté pour cette idée.");

        return Ok(new { message = "Vote enregistré.", nbVotes = result });
    }

    /// <summary>
    /// Supprime le vote d'un auteur sur une idée.
    /// </summary>
    /// <returns>204 No Content | 404 Not Found</returns>
    [HttpDelete("{auteur}")]
    public async Task<IActionResult> SupprimerVote(int ideeId, string auteur)
    {
        var result = await _voteService.SupprimerVoteAsync(ideeId, auteur);

        if (result is null)
            return NotFound($"Idée {ideeId} introuvable.");

        if (result == false)
            return NotFound($"Vote de {auteur} introuvable.");

        return NoContent();
    }
}