using Application.DTOs;

namespace Application.Mocktails.commands.createMocktail;

public class CreateMocktailCommand
{
    public string nom { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal prix { get; set; }
    public string image { get; set; } = string.Empty;
    public List<CreateMocktailIngredientRequest> Ingredients { get; set; } = new();
}