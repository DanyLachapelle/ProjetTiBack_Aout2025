using System.Collections.Generic;
using System.Linq;
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

    // public async Task<IEnumerable<Domain.mocktail>> GetAllAsync()
    // {
    //     return await _context.Mocktails
    //         .Include(m => m.MocktailIngredients)
    //         .ThenInclude(mi => mi.Ingredient)
    //         .ToListAsync();
    // }

    public IEnumerable<Domain.mocktail> GetAllMocktails()
    {
        return _context.Mocktails
            .Include(m => m.MocktailIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .ToList();
    }

    // public async Task<Domain.mocktail?> GetByIdAsync(int id)
    // {
    //     return await _context.Mocktails
    //         .Include(m => m.MocktailIngredients)
    //         .ThenInclude(mi => mi.Ingredient)
    //         .FirstOrDefaultAsync(m => m.id == id);
    // }
    public Domain.mocktail? GetMocktailById(int id)
    {
        return _context.Mocktails
            .Include(m => m.MocktailIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .FirstOrDefault(m => m.id == id);
    }

    // public async Task<Domain.mocktail> CreateAsync(Domain.mocktail mocktail)
    // {
    //     _context.Mocktails.Add(mocktail);
    //     await _context.SaveChangesAsync();
    //     return mocktail;
    // }

    public Domain.mocktail CreateMocktail(Domain.mocktail mocktail)
    {
        _context.Mocktails.Add(mocktail);
        _context.SaveChanges();
        return mocktail;
    }


    // public async Task<Domain.mocktail> UpdateAsync(Domain.mocktail mocktail)
    // {
    //     _context.Mocktails.Update(mocktail);
    //     await _context.SaveChangesAsync();
    //     return mocktail;
    // }
    public Domain.mocktail UpdateMocktail(Domain.mocktail mocktail)
    {
        _context.Mocktails.Update(mocktail);
        _context.SaveChanges();
        return mocktail;
    }


    // public async Task DeleteAsync(int id)
    // {
    //     var mocktail = await _context.Mocktails.FindAsync(id);
    //     if (mocktail != null)
    //     {
    //         _context.Mocktails.Remove(mocktail);
    //         await _context.SaveChangesAsync();
    //     }
    // }
    public void DeleteMocktail(mocktail mocktail)
    {
        _context.Mocktails.Remove(mocktail);
        _context.SaveChanges();
    }


    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Mocktails.AnyAsync(m => m.id == id);
    }

    public async Task<IEnumerable<Domain.Ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }
    
    public Domain.Ingredient? GetIngredientByName(string name)
    {
        return _context.Ingredients.FirstOrDefault(i => i.name == name);
    }

    public Domain.Ingredient AddIngredient(Domain.Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();
        return ingredient;
    }

} 