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

    public async Task<IEnumerable<Domain.Mocktail>> GetAllAsync()
    {
        return await _context.Mocktails
            .Include(m => m.MocktailIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .ToListAsync();
    }

    public async Task<Domain.Mocktail?> GetByIdAsync(int id)
    {
        return await _context.Mocktails
            .Include(m => m.MocktailIngredients)
            .ThenInclude(mi => mi.Ingredient)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Domain.Mocktail> CreateAsync(Domain.Mocktail mocktail)
    {
        _context.Mocktails.Add(mocktail);
        await _context.SaveChangesAsync();
        return mocktail;
    }

    public async Task<Domain.Mocktail> UpdateAsync(Domain.Mocktail mocktail)
    {
        _context.Mocktails.Update(mocktail);
        await _context.SaveChangesAsync();
        return mocktail;
    }

    public async Task DeleteAsync(int id)
    {
        var mocktail = await _context.Mocktails.FindAsync(id);
        if (mocktail != null)
        {
            _context.Mocktails.Remove(mocktail);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Mocktails.AnyAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Domain.ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }
} 