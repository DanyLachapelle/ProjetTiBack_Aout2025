using Application.Ingredient.query.getAllIngredient;
using Application.Utils;

namespace Application.Ingredient.query;

// Processeur centralisé pour les queries relatives aux ingrédients
public class IngredientQueryProcessor
{
    // Handler pour la récupération de tous les ingrédients
    private readonly IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> _ingredientGetAllHandler;
   
    // Injection de dépendance du handler
    public IngredientQueryProcessor(
        IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput> ingredientGetAllHandler)
    {
        _ingredientGetAllHandler = ingredientGetAllHandler;
    }
   
    // Méthode publique pour obtenir tous les ingrédients
    public IngredientGetAllOutput GetAllIngredients(IngredientGetAllQuery query)
    {
        // Délégation au handler spécialisé
        return _ingredientGetAllHandler.Handle(query);
    }
}