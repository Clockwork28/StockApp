using StockApp.DTOs.Stock;
using StockApp.Models;

namespace StockApp.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(Guid id);
        Task<Stock> CreateAsync(CreateStockRequest request);
        Task<Stock?> UpdateAsync(Guid id, UpdateStockRequest request);
        Task<Stock?> DeleteAsync(Guid id);

    }
}

