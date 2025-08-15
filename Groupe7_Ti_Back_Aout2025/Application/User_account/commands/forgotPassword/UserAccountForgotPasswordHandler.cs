using Api.Services;
using Application.Services;
using Application.Utils;
using Infrastructure.User;

namespace Application.User.commands.forgotPassword;

// Handler pour la réinitialisation de mot de passe
public class UserAccountForgotPasswordHandler : ICommandHandler<UserAccountForgotPasswordCommand, UserAccountForgotPasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly IEmailService _emailService;

    // Injection des services nécessaires
    public UserAccountForgotPasswordHandler(IUserRepository userRepository, TokenService tokenService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    // Traitement de la demande de réinitialisation
    public UserAccountForgotPasswordOutput Handle(UserAccountForgotPasswordCommand command)
    {
        // Recherche de l'utilisateur par email (appel synchrone)
        var user = _userRepository.GetUserByEmailAsync(command.Email).GetAwaiter().GetResult();

        if (user != null)
        {
            // Génération du token et construction du lien
            var token = _tokenService.GeneratePasswordResetToken(command.Email);
            var resetLink = $"http://localhost:4200/reset-password?token={token}";

            // Envoi de l'email avec le lien (appel synchrone)
            _emailService.SendEmailAsync(user.Email, "Reset your password", $"Click here: {resetLink}").GetAwaiter().GetResult();
        }

        // Retourne toujours le même message pour éviter l'email fishing
        return new UserAccountForgotPasswordOutput("If your email is associated with an account, you will receive a reset link.");
    }
}