using Application.User.commands.login;
using Application.Utils;

namespace Application.User.commands;

public class UserAccountCommandProcessor
{
    private readonly ICommandHandler<UserAccountLoginCommand,UserAccountLoginOutput> _userLoginHandler;
    
    public UserAccountCommandProcessor(ICommandHandler<UserAccountLoginCommand, UserAccountLoginOutput> userLoginHandler)
    {
        _userLoginHandler = userLoginHandler;
    }
    
    public UserAccountLoginOutput Login(UserAccountLoginCommand command)
    {
        return _userLoginHandler.Handle(command);
    }
}