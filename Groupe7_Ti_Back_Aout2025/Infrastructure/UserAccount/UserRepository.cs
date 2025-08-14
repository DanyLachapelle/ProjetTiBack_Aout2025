using System.Linq;

using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.User;

public class UserRepository:IUserRepository
{
    private AppDbContext _appDbContext;
    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public Domain.User_account GetUserByPseudo(string pseudo)
    { 
        return _appDbContext.Users.FirstOrDefault(u => u.Username == pseudo); 
    }

    public void Save(Domain.User_account userAccount)
    {
        _appDbContext.Users.Update(userAccount);
        _appDbContext.SaveChanges();
    }
    
    public async Task<User_account?> GetUserByEmailAsync(string email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdatePasswordAsync(int userId, string newPassword)
    {
        var user = await _appDbContext.Users.FindAsync(userId);
        if (user != null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _appDbContext.SaveChangesAsync();
        }
    }
}