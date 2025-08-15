using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.changePassword;

// Handles user password change operations
public class UserAccountChangePasswordHandler : ICommandHandler<UserAccountChangePasswordCommand, UserAccountChangePasswordOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserAccountChangePasswordHandler> _logger;

    public UserAccountChangePasswordHandler(IUserRepository userRepository, IMapper mapper, ILogger<UserAccountChangePasswordHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    // Processes password change request
    public UserAccountChangePasswordOutput Handle(UserAccountChangePasswordCommand command)
    {
        _logger.LogInformation("Password change request for user id: {Pseudo}", command.Pseudo);

        var user = _userRepository.GetUserByPseudo(command.Pseudo);

        if (user == null)
        {
            _logger.LogWarning("User not found: {Pseudo}", command.Pseudo);
            throw new InvalidOperationException("Invalid user");
        }

        // Validate current password
        if(!BCrypt.Net.BCrypt.Verify(command.OldPassword, user.Password))
        {
            _logger.LogWarning("Incorrect old password for user: {Pseudo}", command.Pseudo);
            throw new InvalidOperationException("Incorrect old password");
        }
        
        // Update with new hashed password
        user.Password = BCrypt.Net.BCrypt.HashPassword(command.NewPassword);
        _userRepository.Save(user);
        
        _logger.LogInformation("Password changed for user id: {Pseudo}", command.Pseudo);
        return new UserAccountChangePasswordOutput("password changed");
    }
}