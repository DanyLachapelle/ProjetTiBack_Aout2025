namespace Application.SalesItem.query.GetItemBySaleById;

public class GetItemBySaleByIdOutput
{
    public int Id { get; set; }
    public int MocktailId { get; set; }
    public string MocktailName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ItemTotal { get; set; }
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; } // Nouveau champ
}