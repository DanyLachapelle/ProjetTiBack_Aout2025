using Application.DTOs;
using Application.SalesItem.query.GetAllItemBySale;
using Application.Utils;
using Infrastructure.Sale;

public class GetAllItemsBySaleHandler : IQueryHandler<GetAllItemsBySaleQuery, GetAllItemsBySaleOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;

    public GetAllItemsBySaleHandler(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository ?? throw new ArgumentNullException(nameof(saleItemRepository));
    }

    public GetAllItemsBySaleOutput Handle(GetAllItemsBySaleQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        Console.WriteLine($"Handling GetAllItemsBySale for SaleId: {query.SaleId}");

        var items = _saleItemRepository.GetBySaleId(query.SaleId).ToList();

        Console.WriteLine($"Items found: {items.Count}");

        return new GetAllItemsBySaleOutput
        {
            SaleId = query.SaleId,
            Items = items.Select(i => new SaleItemDto
            {
                id = i.Id,
                mocktailId = i.MocktailId,
                mocktailName = i.Mocktail?.name ?? "Unknown",
                quantity = i.Quantity,
                unitPrice = i.Mocktail?.price ?? 0,
                itemTotal = i.ItemTotal
            }).ToList(),
            TotalAmount = items.Sum(i => i.ItemTotal)
        };
    }

}