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
    
    public Domain.UserAccount GetUserByPseudo(string pseudo)
    { 
        return _dbContext.Users.FirstOrDefault(u => u.Username == pseudo); 
    }

    public void Save(Domain.UserAccount userAccount)
    {
        _dbContext.Users.Update(userAccount);
        _dbContext.SaveChanges();
    }
    
    public async Task<UserAccount?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdatePasswordAsync(int userId, string newPassword)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user != null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();
        }
    }
}