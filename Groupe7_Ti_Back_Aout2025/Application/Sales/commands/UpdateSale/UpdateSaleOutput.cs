namespace Application.Sales.commands.UpdateSale;

public class UpdateSaleOutput
{
    public int UpdatedId { get; set; }
    public string? NewTableNumber { get; set; }
    public decimal NewTotalAmount { get; set; }
}