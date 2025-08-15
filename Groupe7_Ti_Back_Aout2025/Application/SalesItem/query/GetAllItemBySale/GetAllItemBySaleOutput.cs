using Application.DTOs;

namespace Application.SalesItem.query.GetAllItemBySale;

public class GetAllItemsBySaleOutput
{
    public int SaleId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}
