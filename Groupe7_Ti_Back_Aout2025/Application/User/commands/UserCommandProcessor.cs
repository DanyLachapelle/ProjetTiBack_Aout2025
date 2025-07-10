using Application.User.commands.login;
using Application.Utils;

namespace Application.User.commands;

public class UserCommandProcessor
{
    private readonly IQueryHandler<UserLoginQuery,UserLoginOutput> _userLoginHandler;
    
    public UserCommandProcessor(IQueryHandler<UserLoginQuery, UserLoginOutput> userLoginHandler)
    {
        _userLoginHandler = userLoginHandler;
    }
    
    public UserLoginOutput Login(UserLoginQuery query)
    {
        return _userLoginHandler.Handle(query);
    }
}