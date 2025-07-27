namespace Application.Sales.query.GetSalesByDate;

public class GetSalesByDateOutput
{
    public DateTime Date { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<SaleDto> Sales { get; set; } = new();
}

public class SaleDto
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; }
    public List<SaleItemDto>? Items { get; set; }
}

public class SaleItemDto
{
    public int MocktailId { get; set; }
    public string MocktailName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ItemTotal { get; set; }
}