namespace Application.DTOs;

public class UpdateMocktailRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<UpdateMocktailIngredientRequest> Ingredients { get; set; } = new List<UpdateMocktailIngredientRequest>();
}

public class UpdateMocktailIngredientRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
} 