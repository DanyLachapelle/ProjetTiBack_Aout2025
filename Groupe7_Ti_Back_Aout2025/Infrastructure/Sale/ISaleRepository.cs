using Domain;

namespace Infrastructure.User.Sale;

public interface ISaleRepository
{
    // Méthodes synchrones
    IEnumerable<Domain.Sale> GetAllSales();
    IEnumerable<Domain.Sale> GetAllSalesWithItems();
    Domain.Sale? GetSaleById(int id);
    Domain.Sale CreateSale(Domain.Sale sale);
    Domain.Sale UpdateSale(Domain.Sale sale);
    void DeleteSale(Domain.Sale sale);
    
    // Méthodes asynchrones
    Task<bool> ExistsAsync(int id);
    
    // Gestion des SaleItems
    IEnumerable<Domain.SaleItem> GetSaleItemsBySaleId(int saleId);
    Domain.SaleItem AddSaleItem(Domain.SaleItem saleItem);
    void RemoveSaleItem(Domain.SaleItem saleItem);
    
    // Méthodes supplémentaires utiles
    decimal GetTotalSales();
    List<Domain.Sale> GetSalesByDateRange(DateTime startDate, DateTime endDate, bool includeItems);
    
    Domain.Sale? GetSaleByIdWithItemsAndMocktails(int id);
    
    Domain.Sale? GetByIdWithItems(int id);

    Domain.Sale? GetSaleWithItems(int saleId);
    IEnumerable<string> GetAllTables(); // Nouvelle méthode pour récupérer toutes les tables
}