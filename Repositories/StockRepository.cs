using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.DTOs.Stock;
using StockApp.Helpers;
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

        public async Task<bool> CheckIfExists(Guid id)
        {
            return await _dbContext.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dbContext.Stocks.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(Guid id)
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;
            _dbContext.Remove(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks =  _dbContext.Stocks.Include(x=>x.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocks = query.SortBy.ToLower() switch
                {
                    "symbol" => query.SortDescending ? stocks.OrderByDescending(x => x.Symbol) : stocks.OrderBy(x => x.Symbol),
                    "companyname" => query.SortDescending ? stocks.OrderByDescending(x => x.CompanyName) : stocks.OrderBy(x => x.CompanyName),
                    _ => stocks
                };
            }

            var skipNum = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skipNum).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Stocks.Include(x=>x.Comments).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol);
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
