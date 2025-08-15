namespace Application.Sales.query.GetSalesById;

public class GetSalesByIdOutput
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string SaleDate { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public int OrderTimer { get; set; } = 15;
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