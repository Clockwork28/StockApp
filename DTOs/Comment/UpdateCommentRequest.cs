using System.ComponentModel.DataAnnotations;

namespace StockApp.DTOs.Comment
{
    public class UpdateCommentRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "Title must be shorter than 50 characters")]

        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        [MaxLength(280, ErrorMessage = "Content muust be shorter than 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
