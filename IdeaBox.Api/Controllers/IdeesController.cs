using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdeaBox.Api.DTOs;
using IdeaBox.Api.Services;

namespace IdeaBox.Api.Controllers;

/**
* Controller REST pour la gestion des idées.
* Délègue la logique métier à IIdeeService.
* GET et POST accessibles sans authentification.
*PUT et DELETE réservés au rôle dev.
*/
[ApiController]
[Route("api/[controller]")]
public class IdeesController : ControllerBase
{
    private readonly IIdeeService _ideeService;
    private readonly ILogger<IdeesController> _logger;

    /**
    * Constructeur — injection du service et du logger.
    */
    public IdeesController(IIdeeService ideeService, ILogger<IdeesController> logger)
    {
        _ideeService = ideeService;
        _logger = logger;
    }

    /**Récupère la liste de toutes les idées.</summary>
    * <returns>200 OK — Liste de IdeeListDto</returns>
    */
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IdeeListDto>>> GetAll()
    {
        var idees = await _ideeService.GetAllAsync();
        return Ok(idees);
    }

    /** <summary>Récupère le détail complet d'une idée.</summary>
    * <returns>200 OK | 404 Not Found</returns>
    */
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<IdeeDetailDto>> GetById(int id)
    {
        var idee = await _ideeService.GetByIdAsync(id);
        if (idee is null)
            return NotFound();
        return Ok(idee);
    }

    /**Crée une nouvelle idée.</summary>
    * <returns>201 Created | 400 Bad Request</returns>
    */
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<IdeeDetailDto>> Create(CreateIdeeDto dto)
    {
        var valeursPossibles = new[] { "basse", "moyenne", "haute" };
        if (!valeursPossibles.Contains(dto.Priorite) || !valeursPossibles.Contains(dto.Difficulte))
        {
            _logger.LogWarning("Priorité ou difficulté invalide : {Priorite} / {Difficulte}", dto.Priorite, dto.Difficulte);
            return BadRequest("Priorité ou difficulté invalide. Valeurs acceptées : basse, moyenne, haute.");
        }

        var idee = await _ideeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = idee.IdIdee }, idee);
    }

    /**Met à jour une idée existante. Réservé au rôle dev.</summary>
    * <returns>204 No Content | 404 Not Found | 400 Bad Request</returns>
    */
    [Authorize(Roles = "dev")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateIdeeDto dto)
    {
        var valeursPossibles = new[] { "basse", "moyenne", "haute" };
        if (!valeursPossibles.Contains(dto.Priorite) || !valeursPossibles.Contains(dto.Difficulte))
        {
            _logger.LogWarning("Priorité ou difficulté invalide : {Priorite} / {Difficulte}", dto.Priorite, dto.Difficulte);
            return BadRequest("Priorité ou difficulté invalide. Valeurs acceptées : basse, moyenne, haute.");
        }

        var succes = await _ideeService.UpdateAsync(id, dto);
        if (!succes)
            return NotFound();

        return NoContent();
    }

    /**Supprime une idée et ses commentaires/votes en cascade. Réservé au rôle dev.</summary>
    * <returns>204 No Content | 404 Not Found</returns>
    */
    [Authorize(Roles = "dev")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var succes = await _ideeService.DeleteAsync(id);
        if (!succes)
            return NotFound();

        return NoContent();
    }
}