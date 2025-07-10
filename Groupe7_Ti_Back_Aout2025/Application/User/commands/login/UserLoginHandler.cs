using Api.Services;
using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.login;

public class UserLoginHandler:IQueryHandler<UserLoginQuery, UserLoginOutput>
{
    public readonly IUserRepository _userRepository;
    public readonly IMapper _mapper;
    private readonly ILogger<UserLoginHandler> _logger;
    public readonly TokenService _tokenService;
    
    public UserLoginHandler(IUserRepository userRepository, IMapper mapper, TokenService tokenService, ILogger<UserLoginHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public UserLoginOutput Handle(UserLoginQuery query)
    {
        _logger.LogInformation("Tentative de connexion avec le pseudo : {Pseudo}", query.pseudo);

        // Recherche l'utilisateur par son pseudo
        var user = _userRepository.GetUserByPseudo(query.pseudo);
       
        if (user == null)
        {
            _logger.LogWarning("Aucun utilisateur trouvé avec le pseudo : {Pseudo}", query.pseudo);
            // Si aucun utilisateur trouvé, on retourne une erreur ou null
            throw new Exception("Invalid credentials");
        }

        var generatedHash = BCrypt.Net.BCrypt.HashPassword(query.password);
        _logger.LogInformation("Mot de passe fourni : {Password}", query.password);
        _logger.LogInformation("Hash généré depuis mot de passe : {Hash}", generatedHash);
        _logger.LogInformation("Hash stocké dans la base : {StoredHash}", user.password);

        // Vérification du mot de passe
        if (!VerifyPassword(query.password, user.password))
        {
            Console.WriteLine("Mot de passe incorrect pour l'utilisateur : " + query.pseudo);
            // Si le mot de passe est incorrect
            throw new Exception("Invalid credentials");
        }

        // // Authentification réussie, mettre à jour l'état de connexion
        // user.IsLoggedIn = true;
        Console.WriteLine("Connexion réussie pour : " + query.pseudo);

        // Sauvegarder les modifications dans la base de données
        _userRepository.Save(user);  // Cette méthode doit être dans ton repository
        
        
        // // Génération du token
        var token = _tokenService.GenerateToken(user);

        // Retourner l'utilisateur avec le token
        var output = _mapper.Map<UserLoginOutput>(user);
        output.Token = token;

        // Retourner l'utilisateur avec ses informations
        return output;
    }
    private bool VerifyPassword(string providedPassword, string storedPasswordHash)
    {
        // _logger.LogInformation("Mot de passe fourni : {Provided}", providedPassword);
        // _logger.LogInformation("Hash stocké : {Hash}", storedPasswordHash);
        // Utiliser bcrypt pour vérifier le mot de passe
        return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
    }
}