using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Services;

/// <summary>
/// Implémentation du service de gestion des votes.
/// Contient la logique métier et les accès à la base de données via EF Core.
/// </summary>
public class VoteService : IVoteService
{
    private readonly AppDbContext _context;
    private readonly ILogger<VoteService> _logger;

    public VoteService(AppDbContext context, ILogger<VoteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int?> VoterAsync(int ideeId, CreateVoteDto dto)
    {
        _logger.LogInformation("Vote sur l'idée {IdeeId} par {Auteur}", ideeId, dto.Auteur);

        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour vote", ideeId);
            return null;
        }

        var dejaVote = await _context.Votes
            .AnyAsync(v => v.IdIdee == ideeId && v.Auteur == dto.Auteur);

        if (dejaVote)
        {
            _logger.LogWarning("Auteur {Auteur} a déjà voté pour l'idée {IdeeId}", dto.Auteur, ideeId);
            return -1;
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

        _logger.LogInformation("Vote enregistré sur l'idée {IdeeId}, total : {NbVotes}", ideeId, nbVotes);

        return nbVotes;
    }

    public async Task<int?> GetNbVotesAsync(int ideeId)
    {
        _logger.LogInformation("Récupération du nombre de votes pour l'idée {IdeeId}", ideeId);

        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour récupération votes", ideeId);
            return null;
        }

        return await _context.Votes.CountAsync(v => v.IdIdee == ideeId);
    }

    public async Task<bool?> SupprimerVoteAsync(int ideeId, string auteur)
    {
        _logger.LogInformation("Suppression du vote de {Auteur} sur l'idée {IdeeId}", auteur, ideeId);

        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour suppression vote", ideeId);
            return null;
        }

        var vote = await _context.Votes
            .FirstOrDefaultAsync(v => v.IdIdee == ideeId && v.Auteur == auteur);

        if (vote is null)
        {
            _logger.LogWarning("Vote de {Auteur} introuvable sur l'idée {IdeeId}", auteur, ideeId);
            return false;
        }

        _context.Votes.Remove(vote);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Vote de {Auteur} supprimé sur l'idée {IdeeId}", auteur, ideeId);
        return true;
    }
}