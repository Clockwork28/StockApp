using System.ComponentModel.DataAnnotations;

namespace StockApp.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool SortDescending { get; set; } = false;
        [Range(1,int.MaxValue, ErrorMessage = "PageNumber must be greater than 0")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 50, ErrorMessage = "PageSize must be between 1-50")]
        public int PageSize { get; set; } = 28;
    }
}
