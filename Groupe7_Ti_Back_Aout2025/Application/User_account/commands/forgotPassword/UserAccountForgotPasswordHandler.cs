using Api.Services;
using Application.Services;
using Application.Utils;
using Infrastructure.User;

namespace Application.User.commands.forgotPassword;

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

    public UserAccountForgotPasswordOutput Handle(UserAccountForgotPasswordCommand command)
    {
        var user = _userRepository.GetUserByEmailAsync(command.Email).GetAwaiter().GetResult();;

        if (user != null)
        {
            var token = _tokenService.GeneratePasswordResetToken(command.Email);

            _emailService.SendEmailAsync(user.Email, "Reset your password", token).GetAwaiter().GetResult();
        }

        return new UserAccountForgotPasswordOutput("If your email is associated with an account, you will receive a reset link.");
    }
}