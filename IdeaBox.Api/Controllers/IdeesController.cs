using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Data;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public IdeesController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/idees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IdeeListDto>>> GetAll()
    {
        var idees = await _context.Idees
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

        return Ok(idees);
    }

    // GET api/idees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IdeeDetailDto>> GetById(int id)
    {
        var idee = await _context.Idees
            .Include(i => i.Commentaires)
            .Include(i => i.Votes)
            .FirstOrDefaultAsync(i => i.IdIdee == id);

        if (idee is null)
            return NotFound();

        var dto = new IdeeDetailDto
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

        return Ok(dto);
    }

    // POST api/idees
    [HttpPost]
    public async Task<ActionResult<IdeeDetailDto>> Create(CreateIdeeDto dto)
    {
        var valeursPossibles = new[] { "basse", "moyenne", "haute" };

        if (!valeursPossibles.Contains(dto.Priorite) || !valeursPossibles.Contains(dto.Difficulte))
            return BadRequest("Priorité ou difficulté invalide. Valeurs acceptées : basse, moyenne, haute.");

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

        return CreatedAtAction(nameof(GetById), new { id = idee.IdIdee }, idee);
    }

    // PUT api/idees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateIdeeDto dto)
    {
        var idee = await _context.Idees.FindAsync(id);
        if (idee is null)
            return NotFound();

        var valeursPossibles = new[] { "basse", "moyenne", "haute" };
        if (!valeursPossibles.Contains(dto.Priorite) || !valeursPossibles.Contains(dto.Difficulte))
            return BadRequest("Priorité ou difficulté invalide. Valeurs acceptées : basse, moyenne, haute.");

        idee.Titre            = dto.Titre;
        idee.Contenu          = dto.Contenu;
        idee.Priorite         = dto.Priorite;
        idee.Difficulte       = dto.Difficulte;
        idee.DateModification = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/idees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var idee = await _context.Idees.FindAsync(id);
        if (idee is null)
            return NotFound();

        _context.Idees.Remove(idee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}