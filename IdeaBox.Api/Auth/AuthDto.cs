namespace IdeaBox.Api.Auth;

/// <summary>
/// Payload de connexion envoyé par le front.
/// </summary>
public class LoginDto
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Réponse retournée après connexion réussie.
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
}