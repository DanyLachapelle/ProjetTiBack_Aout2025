using Application.Ingredient.query.getAllIngredient;
using Application.Utils;

namespace Application.Ingredient.query;

// Centralized processor for ingredient-related queries
public class IngredientQueryProcessor
{
    // Handler for retrieving all ingredients
    private readonly IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> _ingredientGetAllHandler;
   
    // Dependency injection of the handler
    public IngredientQueryProcessor(
        IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> ingredientGetAllHandler)
    {
        _ingredientGetAllHandler = ingredientGetAllHandler;
    }
   
    // Public method to get all ingredients
    public IngredientGetAllOutput GetAllIngredients(IngredientGetAllQuery query)
    {
        // Delegation to the specialized handler
        return _ingredientGetAllHandler.Handle(query);
    }
}