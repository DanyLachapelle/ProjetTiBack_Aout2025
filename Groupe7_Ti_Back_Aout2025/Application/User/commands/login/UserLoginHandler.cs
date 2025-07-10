using Application.Utils;
using AutoMapper;
using Infrastructure.User;

namespace Application.User.commands.login;

public class UserLoginHandler:IQueryHandler<UserLoginQuery, UserLoginOutput>
{
    public readonly IUserRepository _userRepository;
    public readonly IMapper _mapper;
    
    public UserLoginHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public UserLoginOutput Handle(UserLoginQuery query)
    {
        // Recherche l'utilisateur par son pseudo
        var user = _userRepository.GetUserByPseudo(query.pseudo);

        if (user == null)
        {
            // Si aucun utilisateur trouvé, on retourne une erreur ou null
            throw new Exception("Invalid credentials");
        }

        // Vérification du mot de passe
        if (!VerifyPassword(query.password, user.password))
        {
            // Si le mot de passe est incorrect
            throw new Exception("Invalid credentials");
        }

        // // Authentification réussie, mettre à jour l'état de connexion
        // user.IsLoggedIn = true;
       

        // Sauvegarder les modifications dans la base de données
        _userRepository.Save(user);  // Cette méthode doit être dans ton repository
        
        // // Génération du token
        // var token = _tokenService.GenerateToken(user);

        // Retourner l'utilisateur avec le token
        var output = _mapper.Map<UserLoginOutput>(user);
        //output.Token = token;

        // Retourner l'utilisateur avec ses informations
        return output;
    }
    private bool VerifyPassword(string providedPassword, string storedPasswordHash)
    {
        // Utiliser bcrypt pour vérifier le mot de passe
        return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
    }
}