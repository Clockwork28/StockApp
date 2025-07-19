using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StockApp.DTOs.Account
{
    public class NewUserDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Token { get; set; }

    }
}
