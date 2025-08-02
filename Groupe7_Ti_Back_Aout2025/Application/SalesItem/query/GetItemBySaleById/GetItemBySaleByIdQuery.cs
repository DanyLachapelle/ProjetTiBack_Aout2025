namespace Application.SalesItem.query.GetItemBySaleById;

public class GetItemBySaleByIdQuery
{
    public int SaleId { get; }
    public int ItemId { get; }

    public GetItemBySaleByIdQuery(int saleId, int itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }
}