using StockApp.DTOs.Comment;
using StockApp.Models;

namespace StockApp.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllAsync();
        public Task<Comment?> GetByIdAsync(Guid id);
        public Task<Comment> CreateAsync(Comment comment);
        public Task<Comment?> UpdateAsync(Guid id, UpdateCommentRequest request);
        public Task<Comment?> DeleteAsync(Guid id);
    }
}
