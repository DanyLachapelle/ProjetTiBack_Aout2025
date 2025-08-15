using Api.Services;
using Application.User_account.commands.resetPassword;
using Application.Utils;
using Infrastructure.User;

namespace Application.User.commands.resetPassword;

// Handler pour la réinitialisation du mot de passe via token
public class UserAccountResetPasswordHandler : ICommandHandler<UserAccountResetPasswordCommand, UserAccountResetPasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    // Injection des dépendances (repository et service de token)
    public UserAccountResetPasswordHandler(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    // Traitement de la réinitialisation du mot de passe
    public UserAccountResetPasswordOutput Handle(UserAccountResetPasswordCommand command)
    {
        // Vérification et extraction de l'email depuis le token
        var email = _tokenService.GetEmailFromPasswordResetToken(command.Token);
        if (email == null)
            return new UserAccountResetPasswordOutput("Invalid or expired token.");
    
        // Récupération de l'utilisateur (appel synchrone)
        var user = _userRepository.GetUserByEmailAsync(email).GetAwaiter().GetResult();
        if (user == null)
            return new UserAccountResetPasswordOutput("User not found.");
    
        // Mise à jour du mot de passe (appel synchrone)
        _userRepository.UpdatePasswordAsync(user.Id, command.NewPassword).GetAwaiter().GetResult();
    
        return new UserAccountResetPasswordOutput("Password updated successfully.");
    }
}