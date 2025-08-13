using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.changePassword;

public class UserAccountChangePasswordHandler : ICommandHandler<UserAccountChangePasswordCommand, UserAccountChangePasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserAccountChangePasswordHandler> _logger;

    public UserAccountChangePasswordHandler(IUserRepository userRepository, IMapper mapper, ILogger<UserAccountChangePasswordHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public UserAccountChangePasswordOutput Handle(UserAccountChangePasswordCommand command)
    {
        _logger.LogInformation("Password change request for user id : {Pseudo}", command.Pseudo);

        var user = _userRepository.GetUserByPseudo(command.Pseudo);

        if (user == null)
        {
            _logger.LogWarning("User not found: {Pseudo}", command.Pseudo);
            throw new InvalidOperationException("Invalid user");
        }

        if(!BCrypt.Net.BCrypt.Verify(command.OldPassword, user.Password))
        {
            _logger.LogWarning("Incorrect old password for user: {Pseudo}", command.Pseudo);
            throw new InvalidOperationException("Incorrect old password");
        }
        
        var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(command.NewPassword);
        user.Password = hashedNewPassword;
        
        _userRepository.Save(user);
        
        _logger.LogInformation("Password changed for user id : {Pseudo}", command.Pseudo);
        return new UserAccountChangePasswordOutput("password changed");
        
    }
}