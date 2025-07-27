using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure.User;
using Infrastructure.User.Sale;
using Microsoft.EntityFrameworkCore;
using DbContext = Infrastructure.User.DbContext;

namespace Infrastructure.Sale;

public class SaleRepository : ISaleRepository
{
    private readonly DbContext _context;

    public SaleRepository(DbContext context)
    {
        _context = context;
    }

    public IEnumerable<Domain.Sale> GetAllSales()
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Mocktail)
            .ToList();
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
        var query = _context.Sales
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate);

        if (includeItems)
        {
            query = query.Include(s => s.SaleItems)
                .ThenInclude(i => i.Mocktail);
        }

        return query.ToList();
    }
    
    public Domain.Sale? GetSaleByIdWithItemsAndMocktails(int id)
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(i => i.Mocktail) // Chargement des mocktails
            .FirstOrDefault(s => s.Id == id);
    }
    
    public Domain.Sale? GetByIdWithItems(int id)
    {
        return _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(i => i.Mocktail)
            .FirstOrDefault(s => s.Id == id);
    }
}