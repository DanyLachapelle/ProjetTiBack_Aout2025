using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class SaleItem
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int? MocktailId { get; set; } // Nullable pour correspondre à la base de données
    public int Quantity { get; set; }
    public decimal ItemTotal { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Sale Sale { get; set; } = null!;
    public virtual Mocktail? Mocktail { get; set; } // Nullable car MocktailId peut être null
    
    [NotMapped]
    public decimal TotalAmount { get; set; }
}