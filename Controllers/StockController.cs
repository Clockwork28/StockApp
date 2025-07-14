using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.DTOs.Stock;
using StockApp.Interfaces;
using StockApp.Mapper;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController( IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest();
            var stocks = await _stockRepo.GetAllAsync();
            var stockDTOs = stocks.Select(x => x.ToStockDTO());
            return Ok(stockDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var stock = await _stockRepo.GetByIdAsync(id);
            return stock == null ? NotFound() : Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var stock = request.ToStockFromRequest();
            await _stockRepo.CreateAsync(stock);
            return CreatedAtAction(nameof(GetById), new {id = stock.Id}, stock.ToStockDTO());
        }

        [HttpPut("{id:guid}")]

        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStockRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var stock = await _stockRepo.UpdateAsync(id, request);
            return stock == null? NotFound() : Ok(stock.ToStockDTO());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var stock = await _stockRepo.DeleteAsync(id);
            return stock == null ? NotFound() : NoContent();
        }
    }
}
