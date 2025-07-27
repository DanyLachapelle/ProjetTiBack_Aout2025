using Domain;
using Microsoft.EntityFrameworkCore;
using DbContext = Infrastructure.User.DbContext;

namespace Infrastructure.Sale;

    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DbContext _context;

        public SaleItemRepository(DbContext context)
        {
            _context = context;
        }

        public SaleItem GetById(int id)
        {
            return _context.SaleItems
                .Include(si => si.Mocktail)
                .Include(si => si.Sale)
                .FirstOrDefault(si => si.Id == id);
        }

        public IEnumerable<SaleItem> GetBySaleId(int saleId)
        {
            return _context.SaleItems
                .Include(si => si.Mocktail)
                .Where(si => si.SaleId == saleId)
                .ToList();
        }

        public IEnumerable<SaleItem> GetByMocktailId(int mocktailId)
        {
            return _context.SaleItems
                .Include(si => si.Sale)     
                .Include(si => si.Mocktail)
                .Where(si => si.MocktailId == mocktailId)
                .OrderByDescending(si => si.Sale.SaleDate)  
                .ToList();
        }

        public SaleItem Add(SaleItem saleItem)
        {
            _context.SaleItems.Add(saleItem);
            _context.SaveChanges();
            return saleItem;
        }

        public void Update(SaleItem saleItem)
        {
            _context.Entry(saleItem).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(SaleItem saleItem)
        {
            _context.SaleItems.Remove(saleItem);
            _context.SaveChanges();
        }

        public decimal GetTotalRevenueForMocktail(int mocktailId)
        {
            return _context.SaleItems
                .Where(si => si.MocktailId == mocktailId)
                .Sum(si => si.ItemTotal);
        }

        public IEnumerable<SaleItem> GetItemsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.SaleItems
                .Include(si => si.Sale)
                .Include(si => si.Mocktail)
                .Where(si => si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
                .ToList();
        }
}