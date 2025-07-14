using Domain;

namespace Infrastructure.User.Ingredient;

public interface IIngredientRepository
{
    List<ingredient> GetAllIngredient();
    void CreateIngredient(ingredient ingredient);
    void DeleteIngredient(ingredient ingredient);
    ingredient GetIngredientById(int commandId);
    bool UpdateRestockThreshold(int ingredientId, decimal restockThreshold);
    void UpdateQuantityIngredient(ingredient ingredient);
}