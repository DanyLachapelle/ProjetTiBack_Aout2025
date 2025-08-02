namespace Application.DTOs;

public class CreateSaleItemRequest
{
    public int MocktailId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}