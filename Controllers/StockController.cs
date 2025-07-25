﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.DTOs.Stock;
using StockApp.Helpers;
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
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDTOs = stocks.Select(x => x.ToStockDTO()).ToList();
            return Ok(stockDTOs);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = await _stockRepo.GetByIdAsync(id);
            return stock == null ? NotFound() : Ok(stock.ToStockDTO());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStockRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = request.ToStockFromRequest();
            await _stockRepo.CreateAsync(stock);
            return CreatedAtAction(nameof(GetById), new {id = stock.Id}, stock.ToStockDTO());
        }

        [HttpPut("{id:guid}")]
        [Authorize]

        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStockRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = await _stockRepo.UpdateAsync(id, request);
            return stock == null? NotFound() : Ok(stock.ToStockDTO());
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = await _stockRepo.DeleteAsync(id);
            return stock == null ? NotFound() : NoContent();
        }
    }
}
