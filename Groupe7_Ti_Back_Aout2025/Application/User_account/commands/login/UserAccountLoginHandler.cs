using System;
using Api.Services;
using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.login;

// Handler pour le processus de connexion utilisateur
public class UserAccountLoginHandler : ICommandHandler<UserAccountLoginCommand, UserAccountLoginOutput>
{
    // Dépendances injectées
    public readonly IUserRepository _userRepository;
    public readonly IMapper _mapper;
    private readonly ILogger<UserAccountLoginHandler> _logger;
    public readonly TokenService _tokenService;
    
    // Initialisation des services
    public UserAccountLoginHandler(IUserRepository userRepository, IMapper mapper, 
        TokenService tokenService, ILogger<UserAccountLoginHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public UserAccountLoginOutput Handle(UserAccountLoginCommand command)
    {
        _logger.LogInformation("Login attempt with username: {Pseudo}", command.Username);

        // Recherche de l'utilisateur
        var user = _userRepository.GetUserByPseudo(command.Username);

        if (user == null)
        {
            _logger.LogWarning("User not found: {Pseudo}", command.Username);
            throw new InvalidOperationException("Invalid pseudo");
        }

        // Vérification du mot de passe
        if (!VerifyPassword(command.Password, user.Password))
        {
            _logger.LogWarning("Authentication failed for: {Pseudo}", command.Username);
            throw new InvalidOperationException("Invalid password");
        }

        _logger.LogInformation("Authentication successful for: {Pseudo}", command.Username);

        // Génération du token JWT
        var token = _tokenService.GenerateToken(user);

        // Mapping vers l'objet de sortie
        var output = _mapper.Map<UserAccountLoginOutput>(user);
        output.Token = token;

        return output;
    }

    // Méthode utilitaire pour vérifier le mot de passe hashé
    private bool VerifyPassword(string providedPassword, string storedPasswordHash)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
    }
}