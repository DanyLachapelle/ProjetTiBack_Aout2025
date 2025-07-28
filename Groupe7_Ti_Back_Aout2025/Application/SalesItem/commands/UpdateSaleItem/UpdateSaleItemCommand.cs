namespace Application.SalesItem.commands.UpdateSaleItem;

public class UpdateSaleItemCommand
{
    public int SaleId { get; }
    public int ItemId { get; }
    public int NewQuantity { get; }

    public UpdateSaleItemCommand(int saleId, int itemId, int newQuantity)
    {
        SaleId = saleId;
        ItemId = itemId;
        NewQuantity = newQuantity;
    }
}