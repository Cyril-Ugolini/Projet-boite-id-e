using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Services;

/*
 * Service métier pour la gestion des votes.
 * 
 * Responsabilités :
 * - Vérifier les règles métier (ex : un seul vote par utilisateur)
 * - Accéder à la base de données via EF Core
 * - Gérer la création / suppression des votes
 * - Retourner les résultats sous forme simple (compteur, succès, etc.)
 */
public class VoteService : IVoteService
{
    // Contexte EF Core pour accéder à la base de données
    private readonly AppDbContext _context;

    // Logger pour tracer les actions et détecter les erreurs
    private readonly ILogger<VoteService> _logger;

    // Injection des dépendances
    public VoteService(AppDbContext context, ILogger<VoteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /*
     * Ajoute un vote pour une idée
     * 
     * Retour :
     * - null → idée introuvable
     * - -1   → utilisateur a déjà voté
     * - >= 0 → nombre total de votes après ajout
     */
    public async Task<int?> VoterAsync(int ideeId, CreateVoteDto dto)
    {
        _logger.LogInformation("Vote sur l'idée {IdeeId} par {Auteur}", ideeId, dto.Auteur);

        // Vérifie que l'idée existe
        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour vote", ideeId);
            return null;
        }

        // Vérifie si l'utilisateur a déjà voté pour cette idée
        var dejaVote = await _context.Votes
            .AnyAsync(v => v.IdIdee == ideeId && v.Auteur == dto.Auteur);

        if (dejaVote)
        {
            _logger.LogWarning("Auteur {Auteur} a déjà voté pour l'idée {IdeeId}", dto.Auteur, ideeId);
            return -1; // règle métier : un seul vote par utilisateur
        }

        // Création de l'entité Vote (mapping DTO → Model)
        var vote = new Vote
        {
            Auteur       = dto.Auteur,
            IdIdee       = ideeId,
            DateCreation = DateTime.UtcNow
        };

        // Ajout en base
        _context.Votes.Add(vote);
        await _context.SaveChangesAsync();

        // Calcul du nombre total de votes pour cette idée
        var nbVotes = await _context.Votes.CountAsync(v => v.IdIdee == ideeId);

        _logger.LogInformation("Vote enregistré sur l'idée {IdeeId}, total : {NbVotes}", ideeId, nbVotes);

        return nbVotes;
    }

    /*
     * Récupère le nombre total de votes pour une idée
     * 
     * Retour :
     * - null → idée introuvable
     * - nombre de votes sinon
     */
    public async Task<int?> GetNbVotesAsync(int ideeId)
    {
        _logger.LogInformation("Récupération du nombre de votes pour l'idée {IdeeId}", ideeId);

        // Vérifie que l'idée existe
        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour récupération votes", ideeId);
            return null;
        }

        // Compte les votes liés à l'idée
        return await _context.Votes.CountAsync(v => v.IdIdee == ideeId);
    }

    /*
     * Supprime le vote d'un utilisateur pour une idée
     * 
     * Retour :
     * - null  → idée introuvable
     * - false → vote inexistant
     * - true  → suppression réussie
     */
    public async Task<bool?> SupprimerVoteAsync(int ideeId, string auteur)
    {
        _logger.LogInformation("Suppression du vote de {Auteur} sur l'idée {IdeeId}", auteur, ideeId);

        // Vérifie que l'idée existe
        var idee = await _context.Idees.FindAsync(ideeId);
        if (idee is null)
        {
            _logger.LogWarning("Idée {IdeeId} introuvable pour suppression vote", ideeId);
            return null;
        }

        // Recherche du vote de cet utilisateur pour cette idée
        var vote = await _context.Votes
            .FirstOrDefaultAsync(v => v.IdIdee == ideeId && v.Auteur == auteur);

        if (vote is null)
        {
            _logger.LogWarning("Vote de {Auteur} introuvable sur l'idée {IdeeId}", auteur, ideeId);
            return false;
        }

        // Suppression du vote
        _context.Votes.Remove(vote);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Vote de {Auteur} supprimé sur l'idée {IdeeId}", auteur, ideeId);
        return true;
    }
}