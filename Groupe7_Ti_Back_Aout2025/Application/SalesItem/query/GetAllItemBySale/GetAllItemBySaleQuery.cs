namespace Application.SalesItem.query.GetAllItemBySale;

public class GetAllItemsBySaleQuery
{
    public int SaleId { get; }

    public GetAllItemsBySaleQuery(int saleId)
    {
        SaleId = saleId;
    }
}