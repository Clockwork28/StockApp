using Microsoft.AspNetCore.Mvc;
using StockApp.DTOs.Comment;
using StockApp.Interfaces;
using StockApp.Mappers;

namespace StockApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comments = await _commentRepo.GetAllAsync();
            var commentDTOs = comments.Select(x => x.ToCommentDTO());
            return Ok(commentDTOs);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);
            return comment == null ? NotFound() : Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId:guid}")]
        public async Task<IActionResult> Create(Guid stockId, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _stockRepo.CheckIfExists(stockId)) return BadRequest($"Stock with id: {stockId} not found");
            var comment = request.ToCommentFromRequest(stockId);
            await _commentRepo.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDTO());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.UpdateAsync(id, request);
            return comment == null ? NotFound() : Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var comment = await _commentRepo.DeleteAsync(id);
            return comment == null ? NotFound() : NoContent();
        }

        
    }
}
