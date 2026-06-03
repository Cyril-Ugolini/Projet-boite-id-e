using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdeaBox.Api.Auth;

/// <summary>
/// Controller d'authentification.
/// Gère la connexion et la génération du token JWT.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration config, ILogger<AuthController> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <summary>
    /// Authentifie un utilisateur et retourne un token JWT.
    /// </summary>
    /// <returns>200 OK avec token | 401 Unauthorized</returns>
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        _logger.LogInformation("Tentative de connexion pour {Login}", dto.Login);

        // Vérification des identifiants dans appsettings.json
        var users = new[]
        {
            new {
                Login    = _config["Users:User:Login"],
                Password = _config["Users:User:Password"],
                Role     = _config["Users:User:Role"]
            },
            new {
                Login    = _config["Users:Dev:Login"],
                Password = _config["Users:Dev:Password"],
                Role     = _config["Users:Dev:Role"]
            }
        };

        var user = users.FirstOrDefault(u =>
            u.Login == dto.Login && u.Password == dto.Password);

        if (user is null)
        {
            _logger.LogWarning("Echec de connexion pour {Login}", dto.Login);
            return Unauthorized("Identifiants invalides.");
        }

        // Génération du token JWT
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login!),
            new Claim(ClaimTypes.Role, user.Role!)
        };

        var expiration = int.Parse(_config["Jwt:ExpirationHours"]!);

        var token = new JwtSecurityToken(
            issuer:   _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:   claims,
            expires:  DateTime.UtcNow.AddHours(expiration),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("Connexion reussie pour {Login} avec le role {Role}", user.Login, user.Role);

        return Ok(new LoginResponseDto
        {
            Token = tokenString,
            Role  = user.Role!,
            Login = user.Login!
        });
    }
}