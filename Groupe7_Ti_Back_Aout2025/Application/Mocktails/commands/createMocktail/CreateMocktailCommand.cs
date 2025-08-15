using System.Collections.Generic;
using Application.DTOs;

namespace Application.Mocktails.commands.createMocktail;

public class CreateMocktailCommand
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public List<CreateMocktailIngredientRequest> Ingredients { get; set; } = new();
}