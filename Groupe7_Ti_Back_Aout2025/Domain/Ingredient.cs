using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;


public class ingredient
{ 
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public decimal quantity { get; set; }
    public decimal restock_threshold { get; set; }
    public string unit { get; set; } = string.Empty;
    
    // Navigation property pour les mocktails
    public virtual ICollection<MocktailIngredient> MocktailIngredients { get; set; } = new List<MocktailIngredient>();
} 