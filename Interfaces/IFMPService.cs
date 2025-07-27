using StockApp.Models;

namespace StockApp.Interfaces
{
    public interface IFMPService
    {
        Task<Stock?> FindStockBySymbolAsync(string symbol);
    }
}
