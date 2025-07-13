using Domain;

namespace Infrastructure.User.Ingredient;

public interface IIngredientRepository
{
    List<ingredient> GetAllIngredient();
}