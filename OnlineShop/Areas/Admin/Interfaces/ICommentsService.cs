using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface ICommentsService
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<bool> CreateCommentAsync(Comment comment);
        Task<bool> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
        Task<bool> CommentExistsAsync(int id);
    }
}