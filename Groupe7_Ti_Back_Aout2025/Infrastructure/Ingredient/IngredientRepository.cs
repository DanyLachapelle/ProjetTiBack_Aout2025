using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure.Ingredient;
using Infrastructure.User;

namespace Infrastructure.Ingredient;

public class IngredientRepository:IIngredientRepository
{
    private readonly AppDbContext _context;
    
    public IngredientRepository(AppDbContext context)
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
        return _context.Ingredients.FirstOrDefault(i => i.Id == commandId);
    }

    public bool UpdateRestockThreshold(int ingredientId, decimal restockThreshold)
    {
        var ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == ingredientId);
        if (ingredient == null)
            return false;

        ingredient.RestockThreshold = restockThreshold;
        _context.SaveChanges();
        return true;
    }

    public void UpdateQuantityIngredient(Domain.Ingredient ingredient)
    {
        var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
        if (existingIngredient != null)
        {
            existingIngredient.Quantity = ingredient.Quantity;
            existingIngredient.LastModifiedAt = DateTime.Now;
            Console.WriteLine($"Updating ingredient id={ingredient.Id} quantity={ingredient.Quantity} last_modified_at={existingIngredient.LastModifiedAt}");
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Ingredient not found");
        }
    }

    public void DecreaseQuantity(Domain.Ingredient ingredient)
    {
        var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
        if (existingIngredient != null)
        {
            // Pas de décrément ici !
            existingIngredient.Quantity = ingredient.Quantity;
            existingIngredient.LastModifiedAt = ingredient.LastModifiedAt;

            Console.WriteLine($"Decreasing ingredient id={ingredient.Id} new quantity={ingredient.Quantity} last_modified_at={existingIngredient.LastModifiedAt}");

            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Ingredient not found");
        }
    }

}