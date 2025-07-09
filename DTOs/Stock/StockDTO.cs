using System.ComponentModel.DataAnnotations.Schema;

namespace StockApp.DTOs.Stock
{
    public class StockDTO
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal Dividend { get; set; }
    }
}
