using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("ingredient")]
public class Ingredient
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nom")]
    public string Name { get; set; } = string.Empty;

    [Column("quantite_disponible")]
    public decimal Stock { get; set; }

    [Column("seuil_restock")]
    public decimal Limit { get; set; }

    [Column("unite")]
    public string Unit { get; set; } = string.Empty;

    // Navigation property pour les mocktails
    public virtual ICollection<MocktailIngredient> MocktailIngredients { get; set; } = new List<MocktailIngredient>();
} 