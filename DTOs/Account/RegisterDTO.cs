using System.ComponentModel.DataAnnotations;

namespace StockApp.DTOs.Account
{
    public class RegisterDTO
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
