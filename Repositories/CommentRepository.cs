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

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(Guid id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;
            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.Include(x => x.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            var comment = await _dbContext.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(x=>x.Id == id);
            return comment;
        }

        public async Task<Comment?> UpdateAsync(Guid id, UpdateCommentRequest request)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;
            _dbContext.Comments.Entry(comment).CurrentValues.SetValues(request);
            await _dbContext.SaveChangesAsync();
            return comment;

        }
    }
}
