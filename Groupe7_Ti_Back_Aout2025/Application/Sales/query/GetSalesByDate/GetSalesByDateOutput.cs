using Application.DTOs;

namespace Application.Sales.query.GetSalesByDate;

public class GetSalesByDateOutput
{
    public DateTime Date { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<SaleDto> sales { get; set; } = new();
}

public class SaleDto
{
    public int id { get; set; }
    public string tableNumber { get; set; } = string.Empty;
    public decimal totalAmount { get; set; }
    public string saleDate { get; set; } = string.Empty;
    
    public string status { get; set; } = string.Empty;
    
    public int order_timer { get; set; } = 0;
    public List<SaleItemDto>? items { get; set; }
}

