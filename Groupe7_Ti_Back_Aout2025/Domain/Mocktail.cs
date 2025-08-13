using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class Mocktail
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    [Column("price")]
    public decimal Price { get; set; }
    [Column("image")]
    public string? Image { get; set; }
    [Column("forceAvailable")]
    public bool? ForceAvailable { get; set; } = false; // Champ pour forcer la disponibilité (nullable)

    // Navigation property pour les ingrédients
    public virtual ICollection<mocktail_ingredient> MocktailIngredients { get; set; } = new List<mocktail_ingredient>();
} 