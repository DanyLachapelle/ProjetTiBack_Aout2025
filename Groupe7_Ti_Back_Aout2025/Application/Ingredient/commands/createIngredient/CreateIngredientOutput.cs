namespace Application.Ingredient.commands.createIngredient;

public class CreateIngredientOutput
{
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal RestockThreshold { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Allergen { get; set; } = "none"; 
}