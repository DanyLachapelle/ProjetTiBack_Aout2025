using Application.DTOs;
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
            TableNumber = sale.TableNumber ?? string.Empty,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            Status = sale.Status ?? "Pending",
            OrderTimer = sale.OrderTimer,
            
            Items = sale.SaleItems?.Select(i => new SaleItemOutput
            {
                Id = i.Id,
                MocktailId = i.MocktailId,
                MocktailName = i.Mocktail?.Name ?? "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.Mocktail?.Price ?? 0,
                ItemTotal = i.ItemTotal
            }).ToList() ?? new List<SaleItemOutput>()
        };
    }

}