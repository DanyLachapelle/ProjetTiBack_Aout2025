using System.Text.Json.Serialization;

namespace Domain;

public class SaleItem
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int MocktailId { get; set; }
    public int Quantity { get; set; }
    public decimal ItemTotal { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Sale Sale { get; set; } = null!;
    public virtual Mocktail Mocktail { get; set; }
}