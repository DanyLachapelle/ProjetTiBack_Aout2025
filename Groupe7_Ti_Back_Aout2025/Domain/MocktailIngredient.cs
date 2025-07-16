using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("mocktail_ingredient")]
public class MocktailIngredient
{
    [Column("id")]
    public int Id { get; set; }

    [Column("mocktail_id")]
    public int MocktailId { get; set; }

    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    [Column("quantite")]
    public decimal Quantity { get; set; }

    [Column("unite")]
    public string Unit { get; set; } = string.Empty;

    // Navigation properties
    public virtual mocktail Mocktail { get; set; } = null!;
    public virtual ingredient Ingredient { get; set; } = null!;
} 