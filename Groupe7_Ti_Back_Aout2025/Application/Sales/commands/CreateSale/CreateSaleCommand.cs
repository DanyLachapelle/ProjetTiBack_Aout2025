using Application.DTOs;

namespace Application.Sales.commands.CreateSale;

public class CreateSaleCommand
{
    public string TableNumber { get; set; } = string.Empty;
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}
