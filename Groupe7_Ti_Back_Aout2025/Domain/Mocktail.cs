using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("mocktail")]
public class Mocktail
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nom")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("prix")]
    public decimal Price { get; set; }

    [Column("image")]
    public string Image { get; set; } = string.Empty;

    // Navigation property pour les ingrédients
    public virtual ICollection<MocktailIngredient> MocktailIngredients { get; set; } = new List<MocktailIngredient>();
} 