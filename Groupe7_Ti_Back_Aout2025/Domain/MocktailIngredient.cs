using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;


public class mocktail_ingredient
{
    public int id { get; set; }
    public int mocktail_id { get; set; }
    public int ingredient_id { get; set; }
    public decimal quantity { get; set; }
    public string unit { get; set; } = string.Empty;

    // Navigation properties
    [JsonIgnore]
    public virtual mocktail Mocktail { get; set; } = null!;
    public virtual ingredient Ingredient { get; set; } = null!;
} 