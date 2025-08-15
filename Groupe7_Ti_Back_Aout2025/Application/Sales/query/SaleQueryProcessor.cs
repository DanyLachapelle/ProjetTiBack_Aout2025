using Application.Sales.query.GetAllSales;
using Application.Sales.query.GetAllTables;
using Application.Sales.query.GetSalesByDate;
using Application.Sales.query.GetSalesById;
using Application.Utils;

namespace Application.Sales.query;

// Central processor for sales-related queries
public class SalesQueryProcessor
{
    // Handlers for different query types
    private readonly IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput> _getByIdHandler;
    private readonly IQueryHandler<GetSalesByDateQuery, GetSalesByDateOutput> _getByDateHandler;
    private readonly IQueryHandler<GetAllSalesQuery, GetAllSalesOutput> _getAllHandler;
    private readonly IQueryHandler<GetAllTablesQuery, GetAllTablesOutput> _getAllTablesHandler;
    
    // Initialize with all required query handlers
    public SalesQueryProcessor(
        IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput> getByIdHandler,
        IQueryHandler<GetSalesByDateQuery, GetSalesByDateOutput> getByDateHandler,
        IQueryHandler<GetAllSalesQuery, GetAllSalesOutput> getAllHandler,
        IQueryHandler<GetAllTablesQuery, GetAllTablesOutput> getAllTablesHandler)
    {
        _getByIdHandler = getByIdHandler;
        _getByDateHandler = getByDateHandler;
        _getAllHandler = getAllHandler;
        _getAllTablesHandler = getAllTablesHandler;
    }
    
    // Retrieve single sale by ID
    public GetSalesByIdOutput GetSaleById(GetSalesByIdQuery query)
    {
        return _getByIdHandler.Handle(query);
    }
    
    // Get sales for specific date
    public GetSalesByDateOutput GetSalesByDate(GetSalesByDateQuery query)
    {
        return _getByDateHandler.Handle(query);
    }
    
    // Get all sales records
    public GetAllSalesOutput GetAllSales(GetAllSalesQuery query)
    {
        return _getAllHandler.Handle(query);
    }

    // Get all available tables
    public GetAllTablesOutput GetAllTables(GetAllTablesQuery query)
    {
        return _getAllTablesHandler.Handle(query);
    }
}