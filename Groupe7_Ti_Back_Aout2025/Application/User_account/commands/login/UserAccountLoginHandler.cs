using System;
using Api.Services;
using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.login;

// Handles user authentication process
public class UserAccountLoginHandler : ICommandHandler<UserAccountLoginCommand, UserAccountLoginOutput>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserAccountLoginHandler> _logger;
    private readonly TokenService _tokenService;
    
    public UserAccountLoginHandler(
        IUserRepository userRepository, 
        IMapper mapper,
        TokenService tokenService, 
        ILogger<UserAccountLoginHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public UserAccountLoginOutput Handle(UserAccountLoginCommand command)
    {
        _logger.LogInformation("Login attempt: {Pseudo}", command.Username);

        var user = _userRepository.GetUserByPseudo(command.Username);

        // Validate user exists
        if (user == null)
        {
            _logger.LogWarning("User not found: {Pseudo}", command.Username);
            throw new InvalidOperationException("Invalid credentials");
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
        {
            _logger.LogWarning("Authentication failed: {Pseudo}", command.Username);
            throw new InvalidOperationException("Invalid credentials");
        }

        _logger.LogInformation("Authenticated: {Pseudo}", command.Username);

        // Generate JWT and map response
        var output = _mapper.Map<UserAccountLoginOutput>(user);
        output.Token = _tokenService.GenerateToken(user);

        return output;
    }
}