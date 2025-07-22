namespace Application.Ingredient.commands.createIngredient;

public class CreateIngredientCommand
{
    public string name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal restock_threshold { get; set; }
    public string unit { get; set; } = string.Empty;
}