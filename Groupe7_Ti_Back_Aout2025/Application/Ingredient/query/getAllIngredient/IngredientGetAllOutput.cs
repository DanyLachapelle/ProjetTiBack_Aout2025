using System.Collections.Generic;
using Application.DTOs;

namespace Application.Ingredient.query.getAllIngredient;

public class IngredientGetAllOutput
{
    
    public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
    
}