using System.Linq;
using Application.DTOs;
using Application.Utils;
using Domain;
using Infrastructure.Ingredient;

namespace Application.Ingredient.query.getAllIngredient;

// Handler for retrieving all ingredients
public class IngredientGetAllHandler : IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput>
{
    // Repository for data access
    private readonly IIngredientRepository _ingredientRepository;
    
    // Dependency injection
    public IngredientGetAllHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    // Main processing method
    public IngredientGetAllOutput Handle(IngredientGetAllQuery request)
    {
        // Retrieving all ingredients from the repository
        var ingredients = _ingredientRepository.GetAllIngredient();

        // Transforming entities into DTOs
        var ingredientDtos = ingredients.Select(i => new IngredientDto
        {
            Id = i.Id,
            Name = i.Name,
            Quantity = i.Quantity,
            RestockThreshold = i.RestockThreshold,
            Unit = i.Unit,
            Allergen = i.Allergen ?? "none", // Default value for null allergens
            LastModifiedAt = i.LastModifiedAt
        }).ToList();

        // Returning results
        return new IngredientGetAllOutput
        {
            Ingredients = ingredientDtos
        };
    }
}