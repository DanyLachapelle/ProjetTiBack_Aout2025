using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.User.commands.login;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

// Service pour la génération et validation de tokens JWT
public class TokenService
{
    // Configuration des tokens
    private readonly string _jwtSecret; // Clé secrète pour signer les tokens
    private readonly int _resetTokenExpirationMinutes; // Durée de validité des tokens de réinitialisation
    private const double EXPIRY_DURATION_MINUTES = 30; // Durée de validité des tokens d'authentification

    // Constructeur avec injection de la configuration
    public TokenService(IConfiguration configuration)
    {
        // Récupération de la clé JWT depuis la configuration
        _jwtSecret = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Secret not found in configuration");
        // Récupération de la durée de validité des tokens de réinitialisation
        _resetTokenExpirationMinutes = int.Parse(configuration["Jwt:ResetTokenExpirationMinutes"] ?? "15");
    }

    // Construit un token JWT avec les claims fournis
    public string BuildToken(string key, string issuer, UserAccount userAccount)
    {
        // Définition des claims (informations contenues dans le token)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userAccount.Username), // Nom d'utilisateur
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) // Identifiant unique
        };

        // Création de la clé de sécurité
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        // Configuration des credentials pour la signature
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        
        // Création du token avec ses paramètres
        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer, 
            audience: issuer, 
            claims: claims,
            expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), // Date d'expiration
            signingCredentials: credentials);
            
        // Génération de la chaine token
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    // Vérifie si un token est valide
    public bool IsTokenValid(string key, string issuer, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            // Validation du token avec les paramètres
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Vérification de la signature
                    ValidateIssuer = true, // Vérification de l'émetteur
                    ValidateAudience = true, // Vérification du destinataire
                    ValidIssuer = issuer, // Émetteur attendu
                    ValidAudience = issuer, // Destinataire attendu
                    IssuerSigningKey = mySecurityKey, // Clé de vérification
                }, out SecurityToken validatedToken);
        }
        catch
        {
            return false; // Token invalide
        }

        return true; // Token valide
    }

    // Génère un token d'authentification pour un utilisateur
    public string GenerateToken(UserAccount userAccount)
    {
        // Création d'une commande de login (utilisation non claire ici)
        var userLoginQuery = new UserAccountLoginCommand
        {
            Username = userAccount.Username
        };

        // Construction du token avec une clé hardcodée (à éviter en production)
        return BuildToken(
            "JeNeSuisPasConMaisJeMangesDesCaillouxAvecDeLaTerreMésopotamienneHuillée", 
            "www.joydipkanjilal.net", 
            userAccount);
    }
    
    // Génère un token pour la réinitialisation de mot de passe
    public string GeneratePasswordResetToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        // Configuration du token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email), // Email de l'utilisateur
                new Claim("TokenType", "PasswordReset") // Type de token personnalisé
            }),
            Expires = DateTime.UtcNow.AddMinutes(_resetTokenExpirationMinutes), // Expiration
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature // Algorithme de signature
            )
        };
        
        // Création et écriture du token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Extrait l'email d'un token de réinitialisation
    public string? GetEmailFromPasswordResetToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        try
        {
            // Validation du token
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false, // Pas de vérification d'émetteur
                ValidateAudience = false, // Pas de vérification de destinataire
                ValidateIssuerSigningKey = true, // Vérification de la signature
                IssuerSigningKey = new SymmetricSecurityKey(key), // Clé de vérification
                ClockSkew = TimeSpan.Zero // Pas de marge d'expiration
            }, out var validatedToken);

            // Vérification du type de token
            var tokenType = principal.FindFirst("TokenType")?.Value;
            if (tokenType != "PasswordReset")
                return null; // Mauvais type de token

            // Récupération de l'email
            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
        catch
        {
            return null; // Token invalide ou expiré
        }
    }
}