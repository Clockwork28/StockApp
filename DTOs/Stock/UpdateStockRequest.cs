using System.ComponentModel.DataAnnotations;

namespace StockApp.DTOs.Stock
{
    public class UpdateStockRequest
    {
        [Required]
        [MaxLength(5, ErrorMessage = "Symbol must be shorter than 5 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(15, ErrorMessage = "Company name must be shorter than 15 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0, 1000.0)]
        public decimal Dividend { get; set; }
        [Required]
        [MaxLength(15, ErrorMessage = "Industry must be shorter than 15 characters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000)]
        public long MarketCap { get; set; }
    }
}
