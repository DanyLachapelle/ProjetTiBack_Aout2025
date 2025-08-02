namespace Application.SalesItem.commands.RemoveItemFromSale;

public class RemoveItemFromSaleCommand
{
    public int SaleId { get; }
    public int ItemId { get; }
    
    public RemoveItemFromSaleCommand(int saleId, int itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }
    
}