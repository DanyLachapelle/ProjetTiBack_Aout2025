using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class SaleItem
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("sale_id")]
    public int SaleId { get; set; }
    
    [Column("mocktail_id")]
    public int MocktailId { get; set; }
    
    [Column("quantity")]
    public int Quantity { get; set; }
    
    [Column("item_total")]
    public decimal ItemTotal { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Sale Sale { get; set; } = null!;
    public virtual Mocktail Mocktail { get; set; }
    
    [NotMapped]
    public decimal TotalAmount { get; set; }
    
    public void CalculateTotal()
    {
        if (Mocktail == null)
            throw new InvalidOperationException("Mocktail reference is required");

        if (Quantity <= 0)
            throw new ValidationException("Quantity must be positive");

        ItemTotal = Mocktail.Price * Quantity;
        TotalAmount = ItemTotal; // Synchronise les deux propriétés
    }
}