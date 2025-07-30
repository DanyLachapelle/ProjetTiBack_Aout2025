using Application.Sales.query.GetAllSales;
using Application.Sales.query.GetSalesByDate;
using Application.Sales.query.GetSalesById;
using Application.Utils;

namespace Application.Sales.query;

public class SalesQueryProcessor
{
    private readonly IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput> _getByIdHandler;
    private readonly IQueryHandler<GetSalesByDateQuery, GetSalesByDateOutput> _getByDateHandler;
    private readonly IQueryHandler<GetAllSalesQuery, GetAllSalesOutput> _getAllHandler;
    
public SalesQueryProcessor(
        IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput> getByIdHandler,
        IQueryHandler<GetSalesByDateQuery, GetSalesByDateOutput> getByDateHandler,
        IQueryHandler<GetAllSalesQuery, GetAllSalesOutput> getAllHandler)
    {
        _getByIdHandler = getByIdHandler;
        _getByDateHandler = getByDateHandler;
        _getAllHandler = getAllHandler;
    }
    
    public GetSalesByIdOutput GetSaleById(GetSalesByIdQuery query)
    {
        return _getByIdHandler.Handle(query);
    }
    
    public GetSalesByDateOutput GetSalesByDate(GetSalesByDateQuery query)
    {
        return _getByDateHandler.Handle(query);
    }
    
    public GetAllSalesOutput GetAllSales(GetAllSalesQuery query)
    {
        return _getAllHandler.Handle(query);
    }
}