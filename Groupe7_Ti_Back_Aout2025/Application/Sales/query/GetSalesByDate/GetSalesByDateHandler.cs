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
            // 1. Définir la plage de dates (toute la journée)
            var startDate = query.Date.Date;
            var endDate = startDate.AddDays(1).AddTicks(-1);

            // 2. Récupérer les ventes depuis le repository
            var sales = _saleRepository.GetSalesByDateRange(startDate, endDate, query.IncludeItems);

            // 3. Préparer la réponse
            var output = new GetSalesByDateOutput
            {
                Date = startDate,
                TotalSales = sales.Count, // Utilisation de .Count sur List<T>
                TotalRevenue = sales.Sum(s => s.TotalAmount)
            };

            // 4. Mapper les items si demandé
            if (query.IncludeItems)
            {
                output.sales = sales.Select(s => new SaleDto
                {
                    id = s.Id,
                    tableNumber = s.TableNumber ?? string.Empty,
                    totalAmount = s.TotalAmount,
                    saleDate = s.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    status = s.status ?? "Pending",
                    order_timer = s.order_timer,
                    items = s.SaleItems?.Where(si => si != null).Select(i => new SaleItemDto
                    {
                        id = i.Id,
                        mocktailId = i.MocktailId,
                        mocktailName = i.Mocktail?.name ?? "Mocktail supprimé",
                        quantity = i.Quantity,
                        unitPrice = i.Mocktail?.price ?? 0,
                        itemTotal = i.ItemTotal
                    }).ToList() ?? new List<SaleItemDto>()
                }).ToList();
            }
            else
            {
                // Version sans les items
                output.sales = sales.Select(s => new SaleDto
                {
                    id = s.Id,
                    tableNumber = s.TableNumber ?? string.Empty,
                    totalAmount = s.TotalAmount,
                    saleDate = s.SaleDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    status = s.status ?? "Pending",
                    order_timer = s.order_timer,
                    items = new List<SaleItemDto>()
                }).ToList();
            }

            return output;
        }
    }