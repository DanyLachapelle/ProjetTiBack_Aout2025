using Application.Sales.query.GetSalesByDate;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetSalesById;

public class GetSalesByIdHandler : IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesByIdHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public GetSalesByIdOutput Handle(GetSalesByIdQuery query)
    {
        var sale = _saleRepository.GetByIdWithItems(query.Id);

        if (sale == null)
            return null;

        return new GetSalesByIdOutput
        {
            Id = sale.Id,
            TableNumber = sale.TableNumber,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate,
            Items = sale.SaleItems?.Select(i => new SaleItemOutput
            {
                Id = i.Id,
                MocktailId = i.MocktailId,
                MocktailName = i.Mocktail?.name ?? "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.Mocktail?.price ?? 0,
                ItemTotal = i.Quantity * (i.Mocktail?.price ?? 0)
            }).ToList()
        };
    }
}