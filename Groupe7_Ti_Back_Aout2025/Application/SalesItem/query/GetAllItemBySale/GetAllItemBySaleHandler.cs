using Application.DTOs;
using Application.Sales.query.GetSalesByDate;
using Application.Utils;
using Infrastructure.Sale;

namespace Application.SalesItem.query.GetAllItemBySale;

public class GetAllItemsBySaleHandler : IQueryHandler<GetAllItemsBySaleQuery, GetAllItemsBySaleOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetAllItemsBySaleHandler(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository;
    }

    public GetAllItemsBySaleOutput Handle(GetAllItemsBySaleQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var items = _saleItemRepository.GetBySaleId(query.SaleId).ToList();

        return new GetAllItemsBySaleOutput
        {
            SaleId = query.SaleId,
            Items = items.Select(i => new SaleItemDto
            {
                Id = i.Id,
                MocktailId = i.MocktailId,
                MocktailName = i.Mocktail?.name ?? "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.Mocktail?.price ?? 0,
                ItemTotal = i.Quantity * (i.Mocktail?.price ?? 0)
            }).ToList(),
            TotalAmount = items.Sum(i => i.Quantity * (i.Mocktail?.price ?? 0))
        };
    }
}