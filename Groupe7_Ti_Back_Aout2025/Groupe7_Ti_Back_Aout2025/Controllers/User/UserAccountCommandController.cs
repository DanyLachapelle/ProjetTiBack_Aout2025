/*using System;
using System.Security.Claims;
using Application.User_account.commands.resetPassword;
using Application.User.commands;
using Application.User.commands.changePassword;
using Application.User.commands.forgotPassword;
using Application.User.commands.login;
using Application.User.commands.resetPassword;
using Infrastructure.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Groupe7_Ti_Back_Aout2025.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserAccountCommandController: ControllerBase
{
    private readonly UserAccountCommandProcessor _userAccountCommandsProcessor;
    private readonly IUserRepository _userRepository;
    //private readonly ILogger<UserLoginHandler> _logger;
    
    public UserAccountCommandController(UserAccountCommandProcessor userAccountCommandsProcessor, IUserRepository userRepository)
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
    
    //Changing Password
    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(UserAccountChangePasswordOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserAccountChangePasswordOutput), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public IActionResult ChangePassword([FromBody] UserAccountChangePasswordCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var pseudo = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(pseudo))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            command.pseudo = pseudo;

            _userAccountCommandsProcessor.ChangePassword(command);
            return Ok(new { message = "Password updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
    
    // FORGOT PASSWORD
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(UserAccountForgotPasswordOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public IActionResult ForgotPassword([FromBody] UserAccountForgotPasswordCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = _userAccountCommandsProcessor.ForgotPassword(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }


    // RESET PASSWORD
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(UserAccountResetPasswordOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public IActionResult ResetPassword([FromBody] UserAccountResetPasswordCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = _userAccountCommandsProcessor.ResetPassword(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }
}
*/