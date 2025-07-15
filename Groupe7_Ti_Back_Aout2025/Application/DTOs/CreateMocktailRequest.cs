using System.Collections.Generic;

namespace Application.DTOs;

public class CreateMocktailRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<CreateMocktailIngredientRequest> Ingredients { get; set; } = new List<CreateMocktailIngredientRequest>();
}

public class CreateMocktailIngredientRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
} 