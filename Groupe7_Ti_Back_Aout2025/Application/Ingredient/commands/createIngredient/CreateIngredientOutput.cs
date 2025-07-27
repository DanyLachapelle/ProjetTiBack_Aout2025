namespace Application.Ingredient.commands.createIngredient;

public class CreateIngredientOutput
{
    public string name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal restock_threshold { get; set; }
    public string unit { get; set; } = string.Empty;
    public string allergen { get; set; } = "none"; 
}