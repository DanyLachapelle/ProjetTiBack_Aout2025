namespace Application.Sales.query.GetSalesById;

public class GetSalesByIdOutput
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; }
    public List<SaleItemOutput> Items { get; set; } = new();
}

public class SaleItemOutput
{
    public int Id { get; set; }
    public int MocktailId { get; set; }
    public string MocktailName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ItemTotal { get; set; }
}