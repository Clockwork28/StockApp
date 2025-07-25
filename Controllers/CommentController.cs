﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockApp.DTOs.Comment;
using StockApp.Extensions;
using StockApp.Interfaces;
using StockApp.Mappers;
using StockApp.Models;

namespace StockApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comments = await _commentRepo.GetAllAsync();
            var commentDTOs = comments.Select(x => x.ToCommentDTO());
            return Ok(commentDTOs);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);
            return comment == null ? NotFound() : Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId:guid}")]
        [Authorize]
        public async Task<IActionResult> Create(Guid stockId, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _stockRepo.CheckIfExists(stockId)) return BadRequest($"Stock with id: {stockId} not found");
            var username = User.GetUsername();
            if (username == null) return Unauthorized("Username not found");
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return BadRequest("User not found");
            var comment = request.ToCommentFromRequest(stockId);
            comment.AppUserId = appUser.Id;
            await _commentRepo.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDTO());
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.UpdateAsync(id, request);
            return comment == null ? NotFound() : Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.DeleteAsync(id);
            return comment == null ? NotFound() : NoContent();
        }

        
    }
}
