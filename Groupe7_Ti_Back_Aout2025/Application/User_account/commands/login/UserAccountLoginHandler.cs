using System;
using Api.Services;
using Application.Utils;
using AutoMapper;
using Infrastructure.User;
using Microsoft.Extensions.Logging;

namespace Application.User.commands.login;

public class UserAccountLoginHandler:ICommandHandler<UserAccountLoginQuery, UserAccountLoginOutput>
{
    public readonly IUserRepository _userRepository;
    public readonly IMapper _mapper;
    private readonly ILogger<UserAccountLoginHandler> _logger;
    public readonly TokenService _tokenService;
    
    public UserAccountLoginHandler(IUserRepository userRepository, IMapper mapper, TokenService tokenService, ILogger<UserAccountLoginHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }
    
   public UserAccountLoginOutput Handle(UserAccountLoginQuery query)
{
    _logger.LogInformation("Login attempt with username: {Pseudo}", query.username);

    // Find the user by pseudo
    var user = _userRepository.GetUserByPseudo(query.username);

    if (user == null)
    {
        _logger.LogWarning("No user found with pseudo: {Pseudo}", query.username);
        throw new InvalidOperationException("Invalid pseudo");
    }

    _logger.LogInformation("Provided password: {Password}", query.password);
    _logger.LogInformation("Stored password hash: {StoredHash}", user.password);

    // Verify password using bcrypt
    if (!VerifyPassword(query.password, user.password))
    {
        _logger.LogWarning("Incorrect password for user: {Pseudo}", query.username);
        throw new InvalidOperationException("Invalid password");
    }

    _logger.LogInformation("Login successful for: {Pseudo}", query.username);

    _userRepository.Save(user);

    var token = _tokenService.GenerateToken(user);

    var output = _mapper.Map<UserAccountLoginOutput>(user);
    output.Token = token;

    return output;
}

    private bool VerifyPassword(string providedPassword, string storedPasswordHash)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);
    }
}