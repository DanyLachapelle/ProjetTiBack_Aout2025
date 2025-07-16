using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using DbContext = Infrastructure.User.DbContext;

namespace Infrastructure.Mocktail;

public class MocktailRepository : IMocktailRepository
{
    private readonly DbContext _context;
    public MocktailRepository(DbContext context)
    {
        _context = context;
    }
    public List<mocktail> GetAllMocktail()
    {
        return _context.Mocktails.ToList();
    }

    public void CreateMocktail(mocktail mocktail)
    {
        _context.Mocktails.Add(mocktail);
        _context.SaveChanges();
    }

    public void DeleteMocktail(mocktail mocktail)
    {
        _context.Mocktails.Remove(mocktail);
        _context.SaveChanges();
    }

    public mocktail GetMocktailById(int commandId)
    {
        return _context.Mocktails.FirstOrDefault(i => i.Id == commandId);
    }

    public void UpdateMocktail(mocktail mocktail)
    {
        
    }
} 