namespace OnlineShop.Features.Comment.Commands.SubmitComment
{
    public class SubmitCommentCommand
    {
        public string name { get; set; }
        public string email { get; set; }
        public string comment { get; set; }
        public int productId { get; set; }
    }
}
