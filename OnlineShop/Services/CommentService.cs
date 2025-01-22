using OnlineShop.Models.Db;
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

        public SubmitCommentResult SubmitComment(string name, string email, string comment, int productId)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(comment) || productId == 0)
                return new SubmitCommentResult(false, "Please complete your information");

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                return new SubmitCommentResult(false, "Email is not valid");

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

            return new SubmitCommentResult(true, "Your comment submitted successfully");
        }
    }

    public class ProductDetailsViewModel
    {
        public Product? Product { get; set; }
        public List<ProductGalery> Gallery { get; set; } = new();
        public List<Product> NewProducts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }

    public class SubmitCommentResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public SubmitCommentResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}


