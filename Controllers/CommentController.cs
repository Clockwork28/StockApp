using Microsoft.AspNetCore.Authorization;
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
        private readonly IFMPService _fmpService;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
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

        [HttpPost("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create(string symbol, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null) return BadRequest("Stock does not exist");
                else await _stockRepo.CreateAsync(stock);
            }

            var username = User.GetUsername();
            if (username == null) return Unauthorized("Username not found");
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return BadRequest("User not found");
            var comment = request.ToCommentFromRequest(stock.Id);
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
