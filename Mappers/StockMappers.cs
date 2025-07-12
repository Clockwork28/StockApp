using StockApp.DTOs.Stock;
using StockApp.Mappers;
using StockApp.Models;

namespace StockApp.Mapper
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stock)
        {
            return new StockDTO
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                Dividend = stock.Dividend,
                Industry = stock.Industry,
                Comments = stock.Comments.Select(x => x.ToCommentDTO()).ToList()
            }; 
        }

        public static Stock ToStockFromRequest(this CreateStockRequest stock)
        {
            return new Stock
            {
                Id = Guid.NewGuid(),
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                MarketCap = stock.MarketCap,
                Purchase = stock.Purchase,
                Dividend = stock.Dividend,
                Industry = stock.Industry,
            };
        }
    }
}
