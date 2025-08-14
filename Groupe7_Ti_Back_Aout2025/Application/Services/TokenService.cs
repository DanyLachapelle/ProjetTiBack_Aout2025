using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.User.commands.login;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenService
{
    private readonly string _jwtSecret;
    private readonly int _resetTokenExpirationMinutes;
    private const double EXPIRY_DURATION_MINUTES = 30;
    
    public TokenService(IConfiguration configuration)
    {
        _jwtSecret = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Secret not found in configuration");
        _resetTokenExpirationMinutes = int.Parse(configuration["Jwt:ResetTokenExpirationMinutes"] ?? "15");
    }

    public string BuildToken(string key, string issuer, UserAccount userAccount)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userAccount.Username),
            new Claim(ClaimTypes.NameIdentifier,
                Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public bool IsTokenValid(string key, string issuer, string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public string GenerateToken(UserAccount userAccount)
    {
        var userLoginQuery = new UserAccountLoginCommand
        {
            Username = userAccount.Username
        };

        return BuildToken("JeNeSuisPasConMaisJeMangesDesCaillouxAvecDeLaTerreMésopotamienneHuillée", "www.joydipkanjilal.net", userAccount);;
    }
    
    public string GeneratePasswordResetToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("TokenType", "PasswordReset") // utile pour différencier des tokens de connexion
            }),
            Expires = DateTime.UtcNow.AddMinutes(_resetTokenExpirationMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string? GetEmailFromPasswordResetToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // pas de marge supplémentaire
            }, out var validatedToken);

            // Vérifier si le token est bien du type PasswordReset
            var tokenType = principal.FindFirst("TokenType")?.Value;
            if (tokenType != "PasswordReset")
                return null;

            return principal.FindFirst(ClaimTypes.Email)?.Value;
        }
        catch
        {
            return null; // token invalide ou expiré
        }
    }
}