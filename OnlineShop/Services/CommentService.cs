using OnlineShop.Models.Db;
using OnlineShop.Models.ViewModels;
using System.Text.RegularExpressions;

namespace OnlineShop.Services
{
    public class CommentService
    {
        private readonly OnlineShopContext _context;

        public CommentService(OnlineShopContext context)
        {
            _context = context;
        }
         
        public SubmitCommentResultViewModel SubmitComment(string name, string email, string comment, int productId)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(comment) || productId == 0)
                return new SubmitCommentResultViewModel(false, "Please complete your information");

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                return new SubmitCommentResultViewModel(false, "Email is not valid");

            var newComment = new Comment
            {
                Name = name,
                Email = email,
                CommentText = comment,
                ProductId = productId,
                CreateDate = DateTime.Now
            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();

            return new SubmitCommentResultViewModel(true, "Your comment submitted successfully");
        }
    }
}


