using Application.DTOs;

namespace Application.Sales.query.GetSalesByDate;

public class GetSalesByDateOutput
{
    public DateTime Date { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<SaleDto> Sales { get; set; } = new();
}

