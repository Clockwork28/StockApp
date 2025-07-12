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

        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDTOs = comments.Select(x => x.ToCommentDTO());
            return Ok(commentDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            return comment == null ? NotFound() : Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create(Guid stockId, [FromBody] CreateCommentRequest request)
        {
            var comment = await _commentRepo.CreateAsync(stockId, request);
            return comment == null ? BadRequest("Stock doesn't exist") : CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentRequest request)
        {
            var comment = await _commentRepo.UpdateAsync(id, request);
            return comment == null ? NotFound() : Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _commentRepo.DeleteAsync(id);
            return comment == null ? NotFound() : NoContent();
        }

        
    }
}
