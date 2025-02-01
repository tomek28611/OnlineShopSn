using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OnlineShop.Data;
using OnlineShop.ViewModels;
using System.Text.RegularExpressions;

namespace OnlineShop.Features.Comment.Commands.SubmitComment
{
    public class SubmitCommandHandler
    {
        private readonly OnlineShopContext dbcontext;

        public SubmitCommandHandler(OnlineShopContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public SubmitCommentResultViewModel Execute(SubmitCommentCommand command)
        {

            if (string.IsNullOrEmpty(command.name) || string.IsNullOrEmpty(command.email) || string.IsNullOrEmpty(comment) || productId == 0)
                return new SubmitCommentResultViewModel(false, "Please complete your information");

            if (!Regex.IsMatch(command.email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                return new SubmitCommentResultViewModel(false, "Email is not valid");

            var newComment = new Comment
            {
                Name = command.name,
                Email = command.email,
                CommentText = command.comment,
                ProductId = command.productId,
                CreateDate = DateTime.Now
            };

            dbcontext.Comments.Add(newComment);
            dbcontext.SaveChanges();

            return new SubmitCommentResultViewModel(true, "Your comment submitted successfully");

        }
    }
}
