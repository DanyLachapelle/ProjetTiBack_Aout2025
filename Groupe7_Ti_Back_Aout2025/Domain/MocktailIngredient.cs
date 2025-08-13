using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;


public class mocktail_ingredient
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("mocktail_id")]
    public int MocktailId { get; set; }
    
    [Column("ingredient_id")]
    public int IngredientId { get; set; }
    
    [Column("quantity")]
    public decimal Quantity { get; set; }
    
    [Column("unit")]
    public string Unit { get; set; } = string.Empty;

    // Navigation properties
    [JsonIgnore]
    public virtual Mocktail Mocktail { get; set; } = null!;
    public virtual Ingredient Ingredient { get; set; } = null!;
} 