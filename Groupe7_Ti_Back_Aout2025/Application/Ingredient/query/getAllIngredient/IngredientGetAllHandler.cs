using System.Linq;
using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Ingredient;



namespace Application.Ingredient.query.getAllIngredient;

public class IngredientGetAllHandler: IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput>
{
    private readonly IIngredientRepository _ingredientRepository;
    
    public IngredientGetAllHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    public IngredientGetAllOutput Handle(IngredientGetAllQuery request)
    {
        var ingredients = _ingredientRepository.GetAllIngredient();

        var ingredientDtos = ingredients.Select(i => new IngredientDto
        {
            id = i.id,
            name = i.name,
            quantity = i.quantity,
            restock_threshold = i.restock_threshold,
            unit = i.unit,
            allergen = i.allergen ?? "none", 
            last_modified_at = i.last_modified_at
        }).ToList();

        return new IngredientGetAllOutput
        {
            Ingredients = ingredientDtos
        };
    }

    
}