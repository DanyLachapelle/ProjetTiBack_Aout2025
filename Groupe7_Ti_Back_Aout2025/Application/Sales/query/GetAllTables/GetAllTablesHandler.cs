using Application.Sales.query.GetAllTables;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetAllTables;

public class GetAllTablesHandler : IQueryHandler<GetAllTablesQuery, GetAllTablesOutput>
{
    private readonly ISaleRepository _saleRepository;

    public GetAllTablesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public GetAllTablesOutput Handle(GetAllTablesQuery query)
    {
        var output = new GetAllTablesOutput
        {
            Tables = new List<TableDto>
            {
                new TableDto { TableNumber = "T01", DisplayName = "Table T01", IsAvailable = true },
                new TableDto { TableNumber = "T02", DisplayName = "Table T02", IsAvailable = true },
                new TableDto { TableNumber = "T03", DisplayName = "Table T03", IsAvailable = true },
                new TableDto { TableNumber = "T04", DisplayName = "Table T04", IsAvailable = true },
                new TableDto { TableNumber = "T05", DisplayName = "Table T05", IsAvailable = true }
            }
        };

        return output;
    }
}