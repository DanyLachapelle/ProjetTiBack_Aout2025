using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Infrastructure.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mocktail;

public class MocktailRepository : IMocktailRepository
{
    private readonly AppDbContext _context;

    public MocktailRepository(AppDbContext context)
    {
        _context = context;
    }

    

    public IEnumerable<Domain.Mocktail> GetAllMocktails()
    {
        return _context.Mocktails
            .Include(m => m.MocktailIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .ToList();
    }
    
    public Domain.Mocktail? GetMocktailById(int id)
    {
        return _context.Mocktails
            .Include(m => m.MocktailIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .FirstOrDefault(m => m.Id == id);
    }

   

    public Domain.Mocktail CreateMocktail(Domain.Mocktail mocktail)
    {
        _context.Mocktails.Add(mocktail);
        _context.SaveChanges();
        return mocktail;
    }


    
    public Domain.Mocktail UpdateMocktail(Domain.Mocktail mocktail)
    {
        _context.Mocktails.Update(mocktail);
        _context.SaveChanges();
        return mocktail;
    }


    
    public void DeleteMocktail(Domain.Mocktail mocktail)
    {
        _context.Mocktails.Remove(mocktail);
        _context.SaveChanges();
    }


    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Mocktails.AnyAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Domain.Ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }
    
    public Domain.Ingredient? GetIngredientByName(string name)
    {
        return _context.Ingredients.FirstOrDefault(i => i.Name == name);
    }

    public Domain.Ingredient AddIngredient(Domain.Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();
        return ingredient;
    }

} 