﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.Interfaces;
using StockApp.Models;

namespace StockApp.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PortfolioRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _dbContext.Portfolios.Where(u => u.AppUserId == user.Id).Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                Dividend = stock.Stock.Dividend,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
                Comments = stock.Stock.Comments,
                Portfolios = stock.Stock.Portfolios
            }).ToListAsync();
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
        {
            await _dbContext.Portfolios.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio?> DeletePortfolio(AppUser user, string symbol)
        {
            var userPortfolio = await _dbContext.Portfolios.FirstOrDefaultAsync(x=>x.AppUserId == user.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if (userPortfolio == null) return null;
            _dbContext.Portfolios.Remove(userPortfolio);
            await _dbContext.SaveChangesAsync();
            return userPortfolio;
        }
    }
}
