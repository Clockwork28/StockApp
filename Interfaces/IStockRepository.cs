using StockApp.DTOs.Stock;
using StockApp.Helpers;
using StockApp.Models;

namespace StockApp.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(Guid id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(Guid id, UpdateStockRequest request);
        Task<Stock?> DeleteAsync(Guid id);

        Task<bool> CheckIfExists(Guid id);
    }
}

