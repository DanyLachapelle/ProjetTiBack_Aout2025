using System.Linq;

using Domain;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<User_account?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.email == email);
    }

    public async Task UpdatePasswordAsync(int userId, string newPassword)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user != null)
        {
            user.password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();
        }
    }
}