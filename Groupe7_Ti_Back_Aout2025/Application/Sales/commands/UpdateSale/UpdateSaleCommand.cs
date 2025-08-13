using Application.DTOs;

namespace Application.Sales.commands.UpdateSale;

public class UpdateSaleCommand
{
    public int SaleId { get; set; }
    public string? TableNumber { get; set; }
    
    public string? Status { get; set; }
    public List<UpdateSaleRequest>? UpdatedItems { get; set; }
}