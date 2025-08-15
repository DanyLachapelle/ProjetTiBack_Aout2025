using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.changePassword;

// Handler pour la commande de changement de mot de passe utilisateur
public class UserAccountChangePasswordHandler : ICommandHandler<UserAccountChangePasswordCommand, UserAccountChangePasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserAccountChangePasswordHandler> _logger;

    // Injection des dépendances
    public UserAccountChangePasswordHandler(IUserRepository userRepository, IMapper mapper, ILogger<UserAccountChangePasswordHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    // Traitement de la commande
    public UserAccountChangePasswordOutput Handle(UserAccountChangePasswordCommand command)
    {
        _logger.LogInformation("Password change request for user id : {Pseudo}", command.Pseudo);

        // Récupération de l'utilisateur
        var user = _userRepository.GetUserByPseudo(command.Pseudo);

        if (user == null)
        {
            _logger.LogWarning("User not found: {Pseudo}", command.Pseudo);
            throw new InvalidOperationException("Invalid user");
        }

        // Vérification de l'ancien mot de passe
        if(!BCrypt.Net.BCrypt.Verify(command.OldPassword, user.Password))
        {
            _logger.LogWarning("Incorrect old password for user: {Pseudo}", command.Pseudo);
            throw new InvalidOperationException("Incorrect old password");
        }
        
        // Hash et sauvegarde du nouveau mot de passe
        var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(command.NewPassword);
        user.Password = hashedNewPassword;
        
        _userRepository.Save(user);
        
        _logger.LogInformation("Password changed for user id : {Pseudo}", command.Pseudo);
        return new UserAccountChangePasswordOutput("password changed");
    }
}