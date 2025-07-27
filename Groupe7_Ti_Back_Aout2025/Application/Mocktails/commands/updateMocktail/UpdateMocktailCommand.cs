using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.DTOs;

namespace Application.Mocktails.commands.updateMocktail;

public class UpdateMocktailCommand
{
    
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal price { get; set; }
    public string image { get; set; } = string.Empty;
    public List<UpdateMocktailIngredientRequest> Ingredients { get; set; } = new();
    
    [JsonIgnore]
    public int id { get; set; } 
}