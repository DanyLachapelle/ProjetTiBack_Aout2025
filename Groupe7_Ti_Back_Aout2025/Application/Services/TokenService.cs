using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.User.commands.login;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenService
{
    private const double EXPIRY_DURATION_MINUTES = 30;

    public string BuildToken(string key, string issuer, User_account userAccount)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userAccount.username),
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

    public string GenerateToken(User_account userAccount)
    {
        var userLoginQuery = new UserAccountLoginQuery
        {
            username = userAccount.username
        };

        return BuildToken("JeNeSuisPasConMaisJeMangesDesCaillouxAvecDeLaTerreMésopotamienneHuillée", "www.joydipkanjilal.net", userAccount);;
    }
}