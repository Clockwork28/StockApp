using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.DTOs;
using StockApp.Mapper;

namespace StockApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public StockController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks =await  _dbContext.Stocks.ToListAsync();
            var stockDTOs = stocks.Select(x => x.ToStockDTO());
            return Ok(stockDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var stock = _dbContext.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequest request)
        {
            var stockModel = request.ToStockFromRequest();
            _dbContext.Add(stockModel);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDTO());
        }
    }
}
