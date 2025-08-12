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
            id = sale.Id,
            tableNumber = sale.TableNumber ?? string.Empty,
            totalAmount = sale.TotalAmount,
            saleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            status = sale.status ?? "Pending",
            order_timer = sale.order_timer,
            
            items = sale.SaleItems?.Select(i => new SaleItemOutput
            {
                id = i.Id,
                mocktailId = i.MocktailId,
                mocktailName = i.Mocktail?.name ?? "Unknown",
                quantity = i.Quantity,
                unitPrice = i.Mocktail?.price ?? 0,
                itemTotal = i.ItemTotal
            }).ToList() ?? new List<SaleItemOutput>()
        };
    }

}