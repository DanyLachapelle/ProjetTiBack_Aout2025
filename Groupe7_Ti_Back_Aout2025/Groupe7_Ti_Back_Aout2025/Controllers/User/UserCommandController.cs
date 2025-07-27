using System;
using Application.User.commands;
using Application.User.commands.login;
using Infrastructure.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserCommandController: ControllerBase
{
    private readonly UserAccountCommandProcessor _userAccountCommandsProcessor;
    private readonly IUserRepository _userRepository;
    //private readonly ILogger<UserLoginHandler> _logger;
    
    public UserCommandController(UserAccountCommandProcessor userAccountCommandsProcessor, IUserRepository userRepository)
    {
        _userAccountCommandsProcessor = userAccountCommandsProcessor;
        _userRepository = userRepository;
        //_logger = logger;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserAccountLoginCommand), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UserAccountLoginCommand), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public ActionResult<UserAccountLoginCommand> Login([FromBody] UserAccountLoginCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = _userAccountCommandsProcessor.Login(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            // Sends the message like "Invalid pseudo" or "Invalid password"
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Unexpected error during login.");
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}