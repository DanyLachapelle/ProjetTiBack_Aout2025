using Application.User.commands;
using Application.User.commands.login;
using Infrastructure.User;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserCommandController: ControllerBase
{
    private readonly UserCommandProcessor _userCommandsProcessor;
    private readonly IUserRepository _userRepository;
    
    public UserCommandController(UserCommandProcessor userCommandsProcessor, IUserRepository userRepository)
    {
        _userCommandsProcessor = userCommandsProcessor;
        _userRepository = userRepository;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserLoginQuery), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UserLoginQuery), StatusCodes.Status409Conflict)]
    public ActionResult<UserLoginQuery> Login([FromBody] UserLoginQuery query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = _userCommandsProcessor.Login(query);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex.Message);
        }
    }
}