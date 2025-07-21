using System.Linq;

namespace Infrastructure.User;

public class UserRepository:IUserRepository
{
    private DbContext _dbContext;
    public UserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Domain.User_account GetUserByPseudo(string pseudo)
    { 
        return _dbContext.Users.FirstOrDefault(u => u.username == pseudo); 
    }

    public void Save(Domain.User_account userAccount)
    {
        _dbContext.Users.Update(userAccount);
        _dbContext.SaveChanges();
    }
}