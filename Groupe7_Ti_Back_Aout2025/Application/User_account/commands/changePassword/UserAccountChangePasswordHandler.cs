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
        _logger.LogInformation("Password change request for user id : {Pseudo}", command.pseudo);

        var user = _userRepository.GetUserByPseudo(command.pseudo);

        if (user == null)
        {
            _logger.LogWarning("User not found: {Pseudo}", command.pseudo);
            throw new InvalidOperationException("Invalid user");
        }

        if(!BCrypt.Net.BCrypt.Verify(command.oldPassword, user.password))
        {
            _logger.LogWarning("Incorrect old password for user: {Pseudo}", command.pseudo);
            throw new InvalidOperationException("Incorrect old password");
        }
        
        var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(command.newPassword);
        user.password = hashedNewPassword;
        
        _userRepository.Save(user);
        
        _logger.LogInformation("Password changed for user id : {Pseudo}", command.pseudo);
        return new UserAccountChangePasswordOutput("password changed");
        
    }
}