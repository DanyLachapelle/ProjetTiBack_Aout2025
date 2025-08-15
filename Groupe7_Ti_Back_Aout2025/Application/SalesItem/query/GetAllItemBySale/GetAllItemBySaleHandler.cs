using Application.DTOs;
using Application.SalesItem.query.GetAllItemBySale;
using Application.Utils;
using Infrastructure.Sale;

// Handles retrieval of all items belonging to a specific sale
public class GetAllItemsBySaleHandler : IQueryHandler<GetAllItemsBySaleQuery, GetAllItemsBySaleOutput>
{
    private readonly ISaleItemRepository _saleItemRepository;

    // Initializes with required sale item repository dependency
    public GetAllItemsBySaleHandler(ISaleItemRepository saleItemRepository)
    {
        _saleItemRepository = saleItemRepository ?? throw new ArgumentNullException(nameof(saleItemRepository));
    }

    // Retrieves and transforms sale items for the specified sale ID
    public GetAllItemsBySaleOutput Handle(GetAllItemsBySaleQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        // Diagnostic logging (consider using proper logging framework in production)
        Console.WriteLine($"Handling GetAllItemsBySale for SaleId: {query.SaleId}");

        var items = _saleItemRepository.GetBySaleId(query.SaleId).ToList();

        Console.WriteLine($"Items found: {items.Count}");

        // Maps repository items to DTOs and calculates total amount
        return new GetAllItemsBySaleOutput
        {
            SaleId = query.SaleId,
            Items = items.Select(i => new SaleItemDto
            {
                Id = i.Id,
                MocktailId = i.MocktailId,
                MocktailName = i.Mocktail?.Name ?? "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.Mocktail?.Price ?? 0,
                ItemTotal = i.ItemTotal
            }).ToList(),
            TotalAmount = items.Sum(i => i.ItemTotal)
        };
    }
}