using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockApp.Extensions;
using StockApp.Interfaces;
using StockApp.Mappers;
using StockApp.Models;
using StockApp.Services;

namespace StockApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository, IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepository;
            _portfolioRepo = portfolioRepository;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var username = User.GetUsername();
            if (username == null) return NotFound("Username not found");
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return NotFound("User not found");
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(string symbol)
        {
            var username = User.GetUsername();
            if (username == null) return BadRequest("Username not found");
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return BadRequest("User not found");
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null) return BadRequest("Stock does not exist");
                else await _stockRepo.CreateAsync(stock);
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            if (userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add the same stock twice");
                
            var portfolioModel  = new Portfolio
            {
                AppUserId = appUser.Id,
                AppUser = appUser,
                StockId = stock.Id,
                Stock = stock
            };

            portfolioModel = await _portfolioRepo.CreatePortfolio(portfolioModel);
            if (portfolioModel == null) return StatusCode(500, "Couldn't create portfolio");
            return Created();

        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var username = User.GetUsername();
            if (username == null) return Unauthorized("Username not found");
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return BadRequest("User not found");
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null) return BadRequest("Stock not found");
            var deleted = await _portfolioRepo.DeletePortfolio(appUser, symbol);
            if (deleted == null) return NotFound("Stock not in your portfolio");
            return NoContent();
        }
    }
}
