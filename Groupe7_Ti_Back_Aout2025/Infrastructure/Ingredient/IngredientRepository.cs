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

    public void CreateIngredient(ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();
    }

    public void DeleteIngredient(ingredient ingredient)
    {
        _context.Ingredients.Remove(ingredient);
        _context.SaveChanges();
    }

    public ingredient GetIngredientById(int commandId)
    {
        return _context.Ingredients.FirstOrDefault(i => i.id == commandId);
    }

    public bool UpdateRestockThreshold(int ingredientId, decimal restockThreshold)
    {
        var ingredient = _context.Ingredients.FirstOrDefault(i => i.id == ingredientId);
        if (ingredient == null)
            return false;

        ingredient.restock_threshold = restockThreshold;
        _context.SaveChanges();
        return true;
    }

}