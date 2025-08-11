using Api.Services;
using Application.User_account.commands.resetPassword;
using Application.Utils;
using Infrastructure.User;

namespace Application.User.commands.resetPassword;

public class UserAccountResetPasswordHandler : ICommandHandler<UserAccountResetPasswordCommand, UserAccountResetPasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public UserAccountResetPasswordHandler(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public UserAccountResetPasswordOutput Handle(UserAccountResetPasswordCommand command)
    {
        var email = _tokenService.GetEmailFromPasswordResetToken(command.Token);
        if (email == null)
            return new UserAccountResetPasswordOutput("Token invalide ou expiré.");

        var user = _userRepository.GetUserByEmailAsync(email).GetAwaiter().GetResult();;
        if (user == null)
            return new UserAccountResetPasswordOutput("Utilisateur introuvable.");

        _userRepository.UpdatePasswordAsync(user.id, command.NewPassword).GetAwaiter().GetResult();

        return new UserAccountResetPasswordOutput("Mot de passe mis à jour avec succès.");
    }
}