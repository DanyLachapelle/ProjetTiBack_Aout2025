using Domain;

namespace Infrastructure.User.Ingredient;

public class IngredientRepository:IIngredientRepository
{
    private readonly DbContext _context;
    
    public IngredientRepository(DbContext context)
    {
        _context = context;
    }
    public List<ingredient> GetAllIngredient()
    {
        return _context.Ingredients.ToList();
    }
}