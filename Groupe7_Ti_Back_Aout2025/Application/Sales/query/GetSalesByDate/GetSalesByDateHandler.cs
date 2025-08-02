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
                output.Sales = sales.Select(s => new SaleDto
                {
                    Id = s.Id,
                    TableNumber = s.TableNumber,
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate,
                    Status = s.status,
                    order_timer = s.order_timer,
                    Items = s.SaleItems?.Select(i => new SaleItemDto
                    {
                        Id = i.Id,
                        MocktailId = i.MocktailId,
                        MocktailName = i.Mocktail?.name ?? "Inconnu",
                        Quantity = i.Quantity,
                        UnitPrice = i.Mocktail?.price ?? 0,
                        ItemTotal = i.Quantity * (i.Mocktail?.price ?? 0)
                    }).ToList()
                }).ToList();
            }
            else
            {
                // Version sans les items
                output.Sales = sales.Select(s => new SaleDto
                {
                    Id = s.Id,
                    TableNumber = s.TableNumber,
                    TotalAmount = s.TotalAmount
                }).ToList();
            }

            return output;
        }
    }