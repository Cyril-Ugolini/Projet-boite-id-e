using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Controllers;

[ApiController]
[Route("api/idees/{ideeId}/votes")]
public class VotesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<VotesController> _logger;

    public VotesController(AppDbContext context, ILogger<VotesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // POST api/idees/5/votes
    [HttpPost]
    public async Task<IActionResult> Vote(int ideeId, CreateVoteDto dto)
    {
        _logger.LogInformation("Vote sur l'idée {IdeeId} par {Auteur}", ideeId, dto.Auteur);

        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour vote", ideeId);
            return NotFound($"Idée {ideeId} introuvable.");
        }

        var dejaVote = await _context.Votes
            .AnyAsync(v => v.IdIdee == ideeId && v.Auteur == dto.Auteur);

        if (dejaVote)
        {
            _logger.LogWarning("Auteur {Auteur} a déjà voté pour l'idée {IdeeId}", dto.Auteur, ideeId);
            return Conflict("Vous avez déjà voté pour cette idée.");
        }

        var vote = new Vote
        {
            Auteur       = dto.Auteur,
            IdIdee       = ideeId,
            DateCreation = DateTime.UtcNow
        };

        _context.Votes.Add(vote);
        await _context.SaveChangesAsync();

        var nbVotes = await _context.Votes.CountAsync(v => v.IdIdee == ideeId);

        _logger.LogInformation("Vote enregistré sur l'idée {IdeeId}, total votes : {NbVotes}", ideeId, nbVotes);

        return Ok(new { message = "Vote enregistré.", nbVotes });
    }
}