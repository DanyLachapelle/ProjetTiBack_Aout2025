using Application.DTOs;
using Application.Sales.query.GetSalesByDate;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetSalesById;

public class GetSalesByIdHandler : IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput>
{
    private readonly ISaleRepository _saleRepository;

    // Initialize with sale repository dependency
    public GetSalesByIdHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public GetSalesByIdOutput Handle(GetSalesByIdQuery query)
    {
        // Fetch sale with related items from repository
        var sale = _saleRepository.GetByIdWithItems(query.Id);

        // Return null if sale not found
        if (sale == null)
            return null;

        // Map sale data to output DTO
        return new GetSalesByIdOutput
        {
            Id = sale.Id,
            TableNumber = sale.TableNumber ?? string.Empty,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            Status = sale.Status ?? "Pending",
            OrderTimer = sale.OrderTimer,
            
            // Map sale items with null checks
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