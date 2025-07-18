using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class mocktail
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal price { get; set; }
    public string? image { get; set; }

    // Navigation property pour les ingrédients
    public virtual ICollection<mocktail_ingredient> MocktailIngredients { get; set; } = new List<mocktail_ingredient>();
} 