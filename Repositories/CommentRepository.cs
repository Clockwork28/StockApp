using Microsoft.EntityFrameworkCore;
using StockApp.Data;
using StockApp.DTOs.Comment;
using StockApp.Interfaces;
using StockApp.Mappers;
using StockApp.Models;

namespace StockApp.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> CreateAsync(Guid stockId, CreateCommentRequest request)
        {
            var comment = request.ToCommentFromRequest();
            comment.StockId = stockId;
            comment.Stock = await _dbContext.Stocks.FindAsync(stockId);
            if (comment.Stock == null) return null;
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(Guid id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            return comment;
        }

        public async Task<Comment?> UpdateAsync(Guid id, UpdateCommentRequest request)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;
            _dbContext.Comments.Entry(comment).CurrentValues.SetValues(request);
            _dbContext.SaveChanges();
            return comment;

        }
    }
}
