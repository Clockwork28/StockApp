﻿using Microsoft.AspNetCore.Identity;

namespace StockApp.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new();

    }
}
