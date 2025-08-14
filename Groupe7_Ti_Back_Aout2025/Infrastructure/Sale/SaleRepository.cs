using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure.User;
using Infrastructure.User.Sale;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sale;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Domain.Sale> GetAllSales()
    {
        try
        {
            Console.WriteLine("SaleRepository.GetAllSales: Beginning of retrieval");
            
            // Utiliser une requête SQL brute pour gérer les valeurs NULL
            var sales = _context.Sales
                .FromSqlRaw(@"
                    SELECT 
                        id,
                        ISNULL(total_amount, 0) as total_amount,
                        ISNULL(sale_date, GETDATE()) as sale_date,
                        ISNULL(table_number, '') as table_number,
                        ISNULL(status, 'Pending') as status,
                        ISNULL(order_timer, 15) as order_timer
                    FROM SALE
                ")
                .ToList();
            
            Console.WriteLine($"SaleRepository.GetAllSales: {sales.Count} ventes récupérées avec succès");
            return sales;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SaleRepository.GetAllSales: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    public IEnumerable<Domain.Sale> GetAllSalesWithItems()
    {
        try
        {
            Console.WriteLine("SaleRepository.GetAllSalesWithItems: Beginning of retrieval");
            
            // D'abord récupérer toutes les ventes (sans items)
            var sales = GetAllSales().ToList();
            
            Console.WriteLine($"SaleRepository.GetAllSalesWithItems: {sales.Count} ventes récupérées, chargement des items...");
            
            // Puis charger explicitement les items pour chaque vente
            foreach (var sale in sales)
            {
                _context.Entry(sale)
                    .Collection(s => s.SaleItems)
                    .Query()
                    .Include(si => si.Mocktail)
                    .Load();
            }
            
            Console.WriteLine($"SaleRepository.GetAllSalesWithItems: Items chargés pour {sales.Count} ventes");
            return sales;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur dans SaleRepository.GetAllSalesWithItems: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    public Domain.Sale? GetSaleById(int id)
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Mocktail)
            .FirstOrDefault(s => s.Id == id);
    }

    public Domain.Sale CreateSale(Domain.Sale sale)
    {
        _context.Sales.Add(sale);
        _context.SaveChanges();
        return sale;
    }

    public Domain.Sale UpdateSale(Domain.Sale sale)
    {
        _context.Sales.Update(sale);
        _context.SaveChanges();
        return sale;
    }

    public void DeleteSale(Domain.Sale sale)
    {
        _context.Sales.Remove(sale);
        _context.SaveChanges();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Sales.AnyAsync(s => s.Id == id);
    }

    public IEnumerable<Domain.SaleItem> GetSaleItemsBySaleId(int saleId)
    {
        return _context.SaleItems
            .Include(si => si.Mocktail)
            .Where(si => si.SaleId == saleId)
            .ToList();
    }

    public Domain.SaleItem AddSaleItem(Domain.SaleItem saleItem)
    {
        _context.SaleItems.Add(saleItem);
        _context.SaveChanges();
        return saleItem;
    }

    public void RemoveSaleItem(Domain.SaleItem saleItem)
    {
        _context.SaleItems.Remove(saleItem);
        _context.SaveChanges();
    }

    public decimal GetTotalSales()
    {
        // Somme de tous les TotalAmount des ventes
        return _context.Sales.Sum(s => s.TotalAmount);
    }

    public List<Domain.Sale> GetSalesByDateRange(DateTime startDate, DateTime endDate, bool includeItems)
    {
        try
        {
            var query = _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate);

            if (includeItems)
            {
                // Récupérer d'abord les ventes sans les items
                var sales = query.ToList();
                
                // Ensuite, charger les items pour chaque vente séparément
                foreach (var sale in sales)
                {
                    _context.Entry(sale)
                        .Collection(s => s.SaleItems)
                        .Query()
                        .Include(si => si.Mocktail)
                        .Load();
                }
                
                return sales;
            }

            return query.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur dans GetSalesByDateRange: {ex.Message}");
            throw;
        }
    }

    public Domain.Sale? GetSaleByIdWithItemsAndMocktails(int id)
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Mocktail)
            .FirstOrDefault(s => s.Id == id);
    }

    public Domain.Sale? GetByIdWithItems(int id)
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Mocktail)
            .FirstOrDefault(s => s.Id == id);
    }

    public Domain.Sale? GetSaleWithItems(int saleId)
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Mocktail)
            .FirstOrDefault(s => s.Id == saleId);
    }

    public IEnumerable<string> GetAllTables()
    {
        // Récupérer toutes les tables distinctes depuis les ventes
        var existingTables = _context.Sales
            .Where(s => !string.IsNullOrEmpty(s.TableNumber))
            .Select(s => s.TableNumber)
            .Distinct()
            .OrderBy(t => t) // Tri alphabétique simple
            .ToList();

        // Si aucune table n'existe, retourner les tables par défaut
        if (!existingTables.Any())
        {
            return Enumerable.Range(1, 20)
                .Select(i => $"T{i:D2}")
                .ToList();
        }

        // Trier les tables par numéro (T01, T02, T03, etc.)
        return existingTables
            .OrderBy(t => {
                // Extraire le numéro après "T" et le convertir en entier pour un tri numérique
                if (t.StartsWith("T", StringComparison.OrdinalIgnoreCase))
                {
                    var numberPart = t.Substring(1);
                    if (int.TryParse(numberPart, out int number))
                    {
                        return number;
                    }
                }
                return int.MaxValue; // Placer les tables non numériques à la fin
            })
            .ToList();
    }
}