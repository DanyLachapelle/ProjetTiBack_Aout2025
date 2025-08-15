using System.Collections.Generic;
using Domain;

namespace Infrastructure.Ingredient;

public interface IIngredientRepository
{
    List<Domain.Ingredient> GetAllIngredient();
    void CreateIngredient(Domain.Ingredient ingredient);
    void DeleteIngredient(Domain.Ingredient ingredient);
    Domain.Ingredient GetIngredientById(int commandId);
    bool UpdateRestockThreshold(int ingredientId, decimal restockThreshold);
    void UpdateQuantityIngredient(Domain.Ingredient ingredient);
    
    void DecreaseQuantity(Domain.Ingredient commandQuantity);
}