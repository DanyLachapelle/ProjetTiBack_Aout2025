using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure.Ingredient;
using Infrastructure.User;

namespace Infrastructure.Ingredient;

public class IngredientRepository:IIngredientRepository
{
    private readonly DbContext _context;
    
    public IngredientRepository(DbContext context)
    {
        _context = context;
    }
    public List<Domain.Ingredient> GetAllIngredient()
    {
        return _context.Ingredients.ToList();
    }

    public void CreateIngredient(Domain.Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();
    }

    public void DeleteIngredient(Domain.Ingredient ingredient)
    {
        _context.Ingredients.Remove(ingredient);
        _context.SaveChanges();
    }

    public Domain.Ingredient GetIngredientById(int commandId)
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

    public void UpdateQuantityIngredient(Domain.Ingredient ingredient)
    {
        var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.id == ingredient.id);
        if (existingIngredient != null)
        {
            existingIngredient.quantity = ingredient.quantity;
            existingIngredient.last_modified_at = ingredient.last_modified_at;
            Console.WriteLine($"Updating ingredient id={ingredient.id} quantity={ingredient.quantity} last_modified_at={ingredient.last_modified_at}");
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Ingredient not found");
        }
    }

    public void DecreaseQuantity(Domain.Ingredient ingredient)
    {
        var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.id == ingredient.id);
        if (existingIngredient != null)
        {
            // Pas de décrément ici !
            existingIngredient.quantity = ingredient.quantity;
            existingIngredient.last_modified_at = ingredient.last_modified_at;

            Console.WriteLine($"Decreasing ingredient id={ingredient.id} new quantity={ingredient.quantity} last_modified_at={existingIngredient.last_modified_at}");

            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Ingredient not found");
        }
    }

}