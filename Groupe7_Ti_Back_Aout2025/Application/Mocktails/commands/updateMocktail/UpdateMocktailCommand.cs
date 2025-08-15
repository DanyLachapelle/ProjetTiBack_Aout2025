using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.DTOs;

namespace Application.Mocktails.commands.updateMocktail;

public class UpdateMocktailCommand
{
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public bool? ForceAvailable { get; set; } = false;
    public List<UpdateMocktailIngredientRequest> Ingredients { get; set; } = new();
    
    [JsonIgnore]
    public int Id { get; set; } 
}