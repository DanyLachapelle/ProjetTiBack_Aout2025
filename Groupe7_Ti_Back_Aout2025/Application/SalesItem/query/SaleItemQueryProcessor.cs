using Application.SalesItem.query.GetAllItemBySale;
using Application.SalesItem.query.GetItemBySaleById;
using Application.Utils;

namespace Application.SalesItem.query;

public class SaleItemQueryProcessor
{
    private readonly IQueryHandler<GetAllItemsBySaleQuery, GetAllItemsBySaleOutput> _getAllItemsHandler;
    private readonly IQueryHandler<GetItemBySaleByIdQuery, GetItemBySaleByIdOutput> _getItemByIdHandler;

    public SaleItemQueryProcessor(
        IQueryHandler<GetAllItemsBySaleQuery, GetAllItemsBySaleOutput> getAllItemsHandler,
        IQueryHandler<GetItemBySaleByIdQuery, GetItemBySaleByIdOutput> getItemByIdHandler)
    {
        _getAllItemsHandler = getAllItemsHandler;
        _getItemByIdHandler = getItemByIdHandler;
    }

    public GetAllItemsBySaleOutput GetAllItemsBySale(GetAllItemsBySaleQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        return _getAllItemsHandler.Handle(query);
    }

    public GetItemBySaleByIdOutput GetItemBySaleId(GetItemBySaleByIdQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        return _getItemByIdHandler.Handle(query);
    }
}