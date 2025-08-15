namespace Application.Ingredient.commands.DecreaseIngredientQuantity;

public class DecreaseIngredientQuantityOutput
{
    public bool Success { get; set; }
    public decimal Quantity { get; set; }
    public string Message { get; set; } = string.Empty;
}