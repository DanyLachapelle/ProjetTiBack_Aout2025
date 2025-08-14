using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain;

public class SaleItem
{
    private Sale _sale;
    private Mocktail _mocktail;

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
    public virtual Sale Sale
    {
        get => _sale;
        set
        {
            _sale = value;
            SaleId = value?.Id ?? 0;
        }
    }

    public virtual Mocktail Mocktail
    {
        get => _mocktail;
        set
        {
            _mocktail = value;
            MocktailId = value?.Id ?? 0;
        }
    }

    [NotMapped]
    public decimal TotalAmount
    {
        get => ItemTotal;
        set => ItemTotal = value;
    }
}