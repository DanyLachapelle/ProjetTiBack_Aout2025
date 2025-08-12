using Application.Sales.query.GetAllTables;
using Application.Utils;
using Infrastructure.User.Sale;

namespace Application.Sales.query.GetAllTables;

public class GetAllTablesHandler : IQueryHandler<GetAllTablesQuery, GetAllTablesOutput>
{
    private readonly ISaleRepository _saleRepository;

    public GetAllTablesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public GetAllTablesOutput Handle(GetAllTablesQuery query)
    {
        // Récupérer toutes les tables distinctes depuis les ventes
        var allTables = _saleRepository.GetAllTables();

        var output = new GetAllTablesOutput
        {
            Tables = allTables.Select(tableNumber => new TableDto
            {
                TableNumber = tableNumber,
                DisplayName = $"Table {tableNumber}",
                IsAvailable = true
            }).ToList()
        };

        // Si aucune table n'existe, créer les tables par défaut
        if (!output.Tables.Any())
        {
            output.Tables = GenerateDefaultTables();
        }

        // S'assurer que les tables sont triées par numéro
        output.Tables = output.Tables
            .OrderBy(t => {
                // Extraire le numéro après "T" et le convertir en entier pour un tri numérique
                if (t.TableNumber.StartsWith("T", StringComparison.OrdinalIgnoreCase))
                {
                    var numberPart = t.TableNumber.Substring(1);
                    if (int.TryParse(numberPart, out int number))
                    {
                        return number;
                    }
                }
                return int.MaxValue; // Placer les tables non numériques à la fin
            })
            .ToList();

        return output;
    }

    private List<TableDto> GenerateDefaultTables()
    {
        // Générer les tables T01 à T20 par défaut
        var defaultTables = new List<TableDto>();
        
        for (int i = 1; i <= 20; i++)
        {
            defaultTables.Add(new TableDto
            {
                TableNumber = $"T{i:D2}", // T01, T02, T03, etc.
                DisplayName = $"Table T{i:D2}",
                IsAvailable = true
            });
        }

        return defaultTables;
    }
}
