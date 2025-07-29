using Domain;

namespace Infrastructure.Sale;

public interface ISaleItemRepository
{
    SaleItem GetById(int id);
    
    IEnumerable<SaleItem> GetBySaleId(int saleId);
    IEnumerable<SaleItem> GetByMocktailId(int mocktailId);
    SaleItem Add(SaleItem saleItem);
    void Update(SaleItem saleItem);
    void Remove(SaleItem saleItem);
    decimal GetTotalRevenueForMocktail(int mocktailId);
    IEnumerable<SaleItem> GetItemsByDateRange(DateTime startDate, DateTime endDate);
    SaleItem GetByIdWithDetails(int id);

    SaleItem GetByIdWithItems(int commandItemId);
}