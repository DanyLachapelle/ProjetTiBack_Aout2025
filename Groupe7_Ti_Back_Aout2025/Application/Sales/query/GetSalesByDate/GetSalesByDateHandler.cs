using Application.DTOs;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetSalesByDate;

public class GetSalesByDateHandler : IQueryHandler<GetSalesByDateQuery, GetSalesByDateOutput>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesByDateHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public GetSalesByDateOutput Handle(GetSalesByDateQuery query)
    {
        // Set date range for the entire day
        var startDate = query.Date.Date;
        var endDate = startDate.AddDays(1).AddTicks(-1);

        // Fetch sales from repository
        var sales = _saleRepository.GetSalesByDateRange(startDate, endDate, query.IncludeItems);

        // Prepare base response with summary data
        var output = new GetSalesByDateOutput
        {
            Date = startDate,
            TotalSales = sales.Count,
            TotalRevenue = sales.Sum(s => s.TotalAmount)
        };

        // Map sale items if requested
        if (query.IncludeItems)
        {
            output.Sales = sales.Select(s => new SaleDto
            {
                Id = s.Id,
                TableNumber = s.TableNumber ?? string.Empty,
                TotalAmount = s.TotalAmount,
                SaleDate = s.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                Status = s.Status ?? "Pending",
                OrderTimer = s.OrderTimer,
                Items = s.SaleItems?.Where(si => si != null).Select(i => new SaleItemDto
                {
                    Id = i.Id,
                    MocktailId = i.MocktailId,
                    MocktailName = i.Mocktail?.Name ?? "Mocktail deleted",
                    Quantity = i.Quantity,
                    UnitPrice = i.Mocktail?.Price ?? 0,
                    ItemTotal = i.ItemTotal
                }).ToList() ?? new List<SaleItemDto>()
            }).ToList();
        }
        else
        {
            // Basic sale data without items
            output.Sales = sales.Select(s => new SaleDto
            {
                Id = s.Id,
                TableNumber = s.TableNumber ?? string.Empty,
                TotalAmount = s.TotalAmount,
                SaleDate = s.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                Status = s.Status ?? "Pending",
                OrderTimer = s.OrderTimer,
                Items = new List<SaleItemDto>()
            }).ToList();
        }

        return output;
    }
}