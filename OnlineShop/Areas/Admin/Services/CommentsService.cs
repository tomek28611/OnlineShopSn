using OnlineShop.Areas.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;


namespace OnlineShop.Areas.Admin.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly OnlineShopContext _context;

        public CommentsService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            var existingComment = await _context.Comments.FindAsync(comment.Id);
            if (existingComment == null)
            {
                return false;
            }

            existingComment.Name = comment.Name;
            existingComment.Email = comment.Email;
            existingComment.CommentText = comment.CommentText;
            existingComment.ProductId = comment.ProductId;
            existingComment.CreateDate = comment.CreateDate;

            _context.Update(existingComment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CommentExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(e => e.Id == id);
        }
    }
}