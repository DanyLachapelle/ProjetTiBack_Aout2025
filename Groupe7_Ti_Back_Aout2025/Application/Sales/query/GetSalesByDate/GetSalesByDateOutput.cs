using Application.DTOs;

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
    public string SaleDate { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public int OrderTimer { get; set; } = 0;
    public List<SaleItemDto>? Items { get; set; }
}

