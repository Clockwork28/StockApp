using StockApp.Interfaces;
using StockApp.Mapper;
using StockApp.Models;
using System.Text.Json;

namespace StockApp.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public FMPService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }
        public async Task<Stock?> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await _client.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonSerializer.Deserialize<FMPStock[]>(content);
                    var stock = tasks[0];
                    if (stock != null)
                    {
                        return stock.ToStockFromFMP();
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                return null;
            }
        }
    }
}
