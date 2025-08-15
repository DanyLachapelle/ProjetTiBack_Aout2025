namespace Application.SalesItem.commands.RemoveItemFromSale;

public class RemoveItemFromSaleOutput
{
    public bool Success { get; set; }
    public int RemovedItemId { get; set; }
    public decimal NewTotalAmount { get; set; }
}