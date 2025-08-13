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
            Id = i.Id,
            Name = i.Name,
            Quantity = i.Quantity,
            RestockThreshold = i.RestockThreshold,
            Unit = i.Unit,
            Allergen = i.Allergen ?? "none", 
            LastModifiedAt = i.LastModifiedAt,
            // plus besoin de StockStatus ici, c’est calculé dans le DTO
        }).ToList();

        return new IngredientGetAllOutput
        {
            Ingredients = ingredientDtos
        };
    }


    
}