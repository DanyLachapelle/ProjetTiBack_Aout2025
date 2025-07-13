namespace Infrastructure.User;

public class UserRepository:IUserRepository
{
    private DbContext _dbContext;
    public UserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Domain.UserAccount GetUserByPseudo(string pseudo)
    { 
        return _dbContext.Users.FirstOrDefault(u => u.username == pseudo); 
    }

    public void Save(Domain.UserAccount userAccount)
    {
        _dbContext.Users.Update(userAccount);
        _dbContext.SaveChanges();
    }
}