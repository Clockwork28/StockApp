using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.DTOs.Stock;
using StockApp.Interfaces;
using StockApp.Mapper;
using StockApp.Models;

namespace StockApp.Repositories
{
    public class StockRepository : IStockRepository
    {
        private ApplicationDbContext _dbContext;

        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(CreateStockRequest request)
        {
            var stockModel = request.ToStockFromRequest();
            await _dbContext.Stocks.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(Guid id)
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;
            _dbContext.Remove(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _dbContext.Stocks.Include(x=>x.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Stocks.Include(x=>x.Comments).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(Guid id, UpdateStockRequest request)
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;
            _dbContext.Stocks.Entry(stock).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync();
            return stock;

        }
    }
}
