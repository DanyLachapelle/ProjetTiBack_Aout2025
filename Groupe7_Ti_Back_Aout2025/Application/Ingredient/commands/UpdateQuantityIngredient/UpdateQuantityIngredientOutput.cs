namespace Application.Ingredient.commands.UpdateQuantityIngredient;

public class UpdateQuantityIngredientOutput
{
    public bool Success { get; set; }
    public decimal NewQuantity { get; set; }
    public string Message { get; set; } = string.Empty;
}