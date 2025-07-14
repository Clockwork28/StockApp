using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StockApp.DTOs.Comment
{
    public class CreateCommentRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be 3 characters or longer")]
        [MaxLength(50, ErrorMessage ="Title muust be shorter than 50 characters")]

        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Content must be 3 characters or longer")]
        [MaxLength(280, ErrorMessage = "Content muust be shorter than 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}
