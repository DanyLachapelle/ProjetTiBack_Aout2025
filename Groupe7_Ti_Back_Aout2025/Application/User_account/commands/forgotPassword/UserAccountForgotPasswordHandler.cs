using Api.Services;
using Application.Services;
using Application.Utils;
using Infrastructure.User;

namespace Application.User.commands.forgotPassword;

// Handles password reset requests
public class UserAccountForgotPasswordHandler : ICommandHandler<UserAccountForgotPasswordCommand, UserAccountForgotPasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly IEmailService _emailService;

    public UserAccountForgotPasswordHandler(IUserRepository userRepository, TokenService tokenService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    // Processes password reset request
    public UserAccountForgotPasswordOutput Handle(UserAccountForgotPasswordCommand command)
    {
        // Note: Using GetAwaiter().GetResult() for sync handling in command pattern
        var user = _userRepository.GetUserByEmailAsync(command.Email).GetAwaiter().GetResult();

        if (user != null)
        {
            // Generate secure token and reset link
            var token = _tokenService.GeneratePasswordResetToken(command.Email);
            var resetLink = $"http://localhost:4200/reset-password?token={token}";

            _emailService.SendEmailAsync(user.Email, 
                    "Reset your password", 
                    $"Click here: {resetLink}")
                .GetAwaiter().GetResult();
        }

        // Generic response for security
        return new UserAccountForgotPasswordOutput(
            "If your email is associated with an account, you will receive a reset link.");
    }
}