namespace Application.Ingredient.commands.UpdateLimitIngredient;

public class UpdateLimitIngredientCommand
{
    public int Id { get; set; }
    public decimal RestockThreshold { get; set; }
}