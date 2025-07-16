using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;


public class mocktail_ingredient
{
    public int id { get; set; }
    public int mocktail_id { get; set; }
    public int ingredient_id { get; set; }
    public decimal quantite { get; set; }
    public string unite { get; set; } = string.Empty;

    // Navigation properties
    [JsonIgnore]
    public virtual mocktail Mocktail { get; set; } = null!;
    public virtual ingredient Ingredient { get; set; } = null!;
} 