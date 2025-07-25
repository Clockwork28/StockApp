﻿using System.ComponentModel.DataAnnotations.Schema;

namespace StockApp.Models
{
    [Table("Stocks")]
    public class Stock
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName ="decimal(18,2)")]
        public decimal Purchase { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Dividend { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new();
        public List<Portfolio> Portfolios { get; set; } = new();
    }
}
