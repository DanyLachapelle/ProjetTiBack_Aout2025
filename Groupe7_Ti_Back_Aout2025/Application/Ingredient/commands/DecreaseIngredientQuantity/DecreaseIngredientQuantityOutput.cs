namespace Application.Ingredient.commands.DecreaseIngredientQuantity;

public class DecreaseIngredientQuantityOutput
{
    public bool Success { get; set; }
    public decimal quantity { get; set; }
    public string Message { get; set; } = string.Empty;
}